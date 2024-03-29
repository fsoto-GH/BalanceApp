﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Diagnostics;

namespace BalanceApp.App;

/// <summary>
/// Main entry point for the GUI.
/// </summary>
internal partial class App : Form
{
    private readonly TransactionTracker tracker;


    public App()
    {
        InitializeComponent();
        tracker = new();

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
            DatedAmount namedAmount = addBalance.NamedAmount;
            tracker.Add(namedAmount);
            UpdateSummary();
        }
    }

    private void UpdateSummary()
    {
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
            UpdateSummary();
        }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        EditMenu edit = new(tracker);
        edit.ShowDialog();
        UpdateSummary();
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
        SaveFileDialog save = new()
        {
            DefaultExt = ".txt",
            FileName = string.Format("E_{0:00}-{1:00}-{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            Filter = "txt|*.txt"
        };
        if (save.ShowDialog() == DialogResult.OK)
        {
            TransactionPrinter.ExportToFile(tracker, save.FileName);
        }
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
        if (open.ShowDialog() == DialogResult.OK)
        {
            if (doDelete)
            {
                tracker.ClearTransactions();
            }

            foreach (string fileName in open.FileNames)
            {
                var res = loadFromFile(fileName);
                additions += res.Additions;
                totalLinesRead += res.TotalLines;
            }
            UpdateSummary();

            if (additions > 0)
            {
                MessageBox.Show(string.Format("Successfully added {0}/{1} item(s)!", additions, totalLinesRead), "Success");
            }
            else
            {
                MessageBox.Show("No additions made. Check if the file was empty or malformed.\nEach line should follow: \"[Balance|Payment|Cashback],Name,Amount\"");
            }
        }
    }

    private FileImportResult loadFromFile(string fileName)
    {
        int additions = 0;
        string[] lines = File.ReadAllLines(fileName);

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

            if (!(double.TryParse(comps[2], out double amount) && tracker.Categories.Contains(cat)))
            {
                Debug.WriteLine($"Unable to parse: '{line}'");
            }

            tracker.Add(new(name, amount, cat));
            additions++;
        }
        return new FileImportResult
        {
            Additions = additions,
            TotalLines = lines.Length
        };
    }
}
