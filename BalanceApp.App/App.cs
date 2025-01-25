using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Diagnostics;
using System.Collections.Generic;

namespace BalanceApp.App;

/// <summary>
/// Main entry point for the GUI.
/// </summary>
internal partial class App : Form
{
    private readonly TransactionTracker tracker;
    private bool _hasUnsavedChanges;

    public App()
    {
        InitializeComponent();
        tracker = new();
        _hasUnsavedChanges = false;
        if (ApplicationDeployment.IsNetworkDeployed)
        {
            Text = $"Balance App [v. {ApplicationDeployment.CurrentDeployment.CurrentVersion}]";
        }
    }

    private void ShowAddDatedAmountDialog(string category)
    {
        AddEditNamedAmount addBalance = new(category);
        DialogResult result = addBalance.ShowDialog();
        if (result == DialogResult.OK)
        {
            _hasUnsavedChanges = true;
            DatedAmount namedAmount = addBalance.NamedAmount;
            tracker.Add(namedAmount);
            UpdateSummary();
        }
    }

    private void UpdateLastSavedStatus()
    {
        string prefix = "Last Saved: ";
        string dirtySuffix = "*";
        string formattedDateTime = $"{DateTime.Now:MM/dd/yyyy @ hh:mm:ss tt}";
        string curr_status = tsSaveStatus.Text;

        // We have saved and is marked dirty state but with no more unsaved changes.
        if (curr_status.StartsWith(prefix) && curr_status.EndsWith(dirtySuffix) && !_hasUnsavedChanges)
        {
            tsSaveStatus.Text = $"{prefix}{formattedDateTime}";
        } else if (curr_status.StartsWith(prefix) && !curr_status.EndsWith(dirtySuffix) && _hasUnsavedChanges)
        {
            tsSaveStatus.Text = $"{curr_status}*";
        }
    }

    private void UpdateSummary()
    {
        UpdateLastSavedStatus();
        txtSummary.Text = TransactionPrinter.Summary(tracker);
        btnClear.Enabled = tracker.HasTransactions;
        btnEdit.Enabled = tracker.HasTransactions;
        btnExport.Enabled = tracker.HasTransactions;
        btnPrintTxt.Enabled = tracker.HasTransactions;
    }

    private void btnAddNamedAmount_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;

        if (btn is null)
            return;

        switch (btn.Name)
        {
            case "btnAddBalance":
                ShowAddDatedAmountDialog("Balance");
                break;
            case "btnAddPayment":
                ShowAddDatedAmountDialog("Payment");
                break;
            case "btnAddCashback":
                ShowAddDatedAmountDialog("Cashback");
                break;
        }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show("Are you sure you want to clear all?", "Clear All?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result == DialogResult.Yes)
        {
            tracker.ClearTransactions();
            _hasUnsavedChanges = false;
            UpdateSummary();
        }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        EditMenu edit = new(tracker);
        if (edit.ShowDialog() == DialogResult.OK)
        {
            _hasUnsavedChanges = true;
            UpdateSummary();
        }
    }

    private void btnPrintTxt_Click(object sender, EventArgs e)
    {
        SaveFileDialog save = new()
        {
            DefaultExt = ".txt",
            FileName = string.Format("{0:00}-{1:00}-{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            Filter = "txt|*.txt"
        };
        if (save.ShowDialog() == DialogResult.OK)
        {
            TransactionPrinter.SaveToFile(tracker, save.FileName);
        }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
        bool isSaved = ExportToFile();
        if (isSaved)
        {
            _hasUnsavedChanges = false;
            UpdateSummary();
        }
    }

    /// <summary>
    /// Reponsible for opening a FileSave modal and saving the transactions to a file.
    /// </summary>
    /// <returns>true if save was made else false</returns>
    private bool ExportToFile()
    {
        SaveFileDialog save = new()
        {
            DefaultExt = ".txt",
            FileName = string.Format("E_{0:00}-{1:00}-{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            Filter = "txt|*.txt"
        };
        if (save.ShowDialog() == DialogResult.OK)
        {
            TransactionPrinter.ExportToFile(tracker, save.FileName);

            string prefix = "Last Saved: ";
            string formattedDateTime = $"{DateTime.Now:MM/dd/yyyy @ hh:mm:ss tt}";
            tsSaveStatus.Text = $"{prefix}{formattedDateTime}";
            return true;
        }

        return false;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
        bool doDelete = false;
        // ask user if they wish to delete any existing transactions
        if (tracker.HasTransactions)
        {
            DialogResult box = MessageBox.Show("There are existing transactions. Would you like to clear them?", "Clear Existing Transactions?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (box == DialogResult.Cancel) return;

            doDelete = box == DialogResult.Yes;
        }

        OpenFileDialog open = new()
        {
            DefaultExt = ".txt",
            Filter = "txt|*.txt",
            Multiselect = true
        };

        int additions = 0;
        int totalLinesRead = 0;
        List<DatedAmount> allReadTransactions = new();
        if (open.ShowDialog() == DialogResult.OK)
        {
            foreach (string fileName in open.FileNames)
            {
                var res = LoadFromFile(fileName);
                allReadTransactions.AddRange(res.ReadTransactions);
                additions += res.Additions;
                totalLinesRead += res.TotalLines;
            }

            if (doDelete)
            {
                tracker.ClearTransactions();
                // we only have file-loaded transactions
                _hasUnsavedChanges = true;
            } else
            {
                _hasUnsavedChanges = _hasUnsavedChanges && tracker.HasTransactions;
            }
            tracker.AddTransactions(allReadTransactions);

            if (additions > 0)
            {
                MessageBox.Show(string.Format("Successfully added {0}/{1} item(s)!", additions, totalLinesRead), "Success");
            }
            else
            {
                MessageBox.Show("No additions made. Check if the file was empty or malformed." +
                                "\nEach line should follow: \"Category,Name,Amount\"" +
                                $"\n- Category: one of '{string.Join(",", tracker.Categories)}'" +
                                $"\n- Name: description of the record" +
                                $"\n- Amount: of the record in $#.##");
            }

            UpdateSummary();
        }
    }

    private FileImportResult LoadFromFile(string fileName)
    {
        int additions = 0;
        string[] lines = File.ReadAllLines(fileName).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        List<DatedAmount> readTransactions = new();

        foreach (string line in lines)
        {
            string[] comps = line.Split(',');

            if (comps.Length != 3)
            {
                Debug.WriteLine($"Incorrect number of arguments in: '[File: '{fileName}']: '{line}'");
                continue;
            }

            string cat = comps[0];
            string name = comps[1];

            if (double.TryParse(comps[2], out double amount) && tracker.Categories.Contains(cat))
            {
                readTransactions.Add(new(name, amount, cat));
                additions++;
            }
            else
            {
                Debug.WriteLine($"Unable to parse: '{line}'");
            }
        }
        return new FileImportResult
        {
            Additions = additions,
            TotalLines = lines.Length,
            ReadTransactions = readTransactions
        };
    }

    private void App_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (tracker.HasTransactions && _hasUnsavedChanges)
        {
            if (MessageBox.Show("You have unsaved transactions, are you sure you want to close?", "Unsaved Transactions", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==  DialogResult.No)
            {
                // The user did not fully save the transactions, so we cancel to reprompt.
                if (!ExportToFile())
                {
                    e.Cancel = true;
                }
            } 
        }

    }
}
