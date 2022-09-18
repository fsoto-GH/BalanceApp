using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PaymentBalanceApplication;

/// <summary>
/// This form is used to narrow down on the transactions and choose one to edit or delete.
/// </summary>
internal partial class EditMenu : Form
{
    private readonly TransactionTracker _transactionTracker;


    public EditMenu(TransactionTracker transactionTracker)
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
        
        _transactionTracker = transactionTracker;
        ResetDropDowns();
    }

    private void ResetDropDowns()
    {
        cbCategory.Items.Clear();

        foreach(var type in _transactionTracker.Categories)
        {
            if (_transactionTracker.HasAnyOfType(type))
            {
                cbCategory.Items.Add(new TransactionTypeDropDownValue { Name = $"{type}s", Value = type});
            }
        }
        cbCategory.DisplayMember = "Name";
        cbCategory.ValueMember = "Value";

        if (cbCategory.Items.Count > 0)
        {
            cbCategory.SelectedIndex = 0;
        }
        else
        {
            cbSelect.DataSource = null;
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        _transactionTracker.Remove((DatedAmount)cbSelect.SelectedItem);
        ResetDropDowns();
        if (!_transactionTracker.HasTransactions)
        {
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            cbCategory.Enabled = false;
            cbSelect.Enabled = false;
        }
    }

    private void btnDone_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        AddEditNamedAmount edit = new AddEditNamedAmount((DatedAmount)cbSelect.SelectedItem, cbCategory.Text, "Make Edit", true);
        DialogResult result = edit.ShowDialog();
        string selCat = cbCategory.SelectedValue as string;
        if(result == DialogResult.OK)
        {
            cbSelect.SelectedItem = edit.NamedAmount;
        }

        ResetDropDowns();
        if (!_transactionTracker.HasAnyOfType(selCat))
        {
            cbCategory.SelectedIndex = 0;
        } else
        {
            cbCategory.SelectedIndex = cbCategory.FindStringExact(selCat);
        }
    }

    private void Edit_Load(object sender, EventArgs e)
    {
        cbCategory.SelectedIndex = 0;
    }

    private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        TransactionTypeDropDownValue val = cbCategory.SelectedItem as TransactionTypeDropDownValue;
        cbSelect.DataSource = _transactionTracker.GetTransactionsOfType(val.Value).OrderBy(x => x.AmountName).ToArray();
    }
}
