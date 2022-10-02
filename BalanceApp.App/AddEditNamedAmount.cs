using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BalanceApp.App;

/// <summary>
/// This form is used to add or edit a transaction.
/// </summary>
internal partial class AddEditNamedAmount : Form
{
    public DatedAmount NamedAmount { get; set; }
    private readonly string category;


    public AddEditNamedAmount(string category = "")
    {
        InitializeComponent();
        NamedAmount = new();
        Text = $"Add a {category}";
        btnAdd.Text = "Add";
        this.category = category;
        AdjustCategoryDisplay(false);
    }

    public AddEditNamedAmount(DatedAmount namedAmount, string category = "", string btnAcceptText = "Add", bool showCategory = false)
    {
        InitializeComponent();
        NamedAmount = namedAmount;
        txtAmount.Text = $"{namedAmount.Amount:0.00}";
        txtName.Text = namedAmount.AmountName;
        Text = $"Editing from {category}";
        btnAdd.Text = btnAcceptText;
        this.category = category;
        AdjustCategoryDisplay(showCategory);
    }

    private void AdjustCategoryDisplay(bool doDisplay)
    {
        if (doDisplay)
        {
            cbCategory.SelectedIndex = cbCategory.FindStringExact(category);
            MinimumSize = new Size(310, 220);
        }
        else
        {
            this.Controls.Remove(lblCategory);
            this.Controls.Remove(cbCategory);
            this.Width = 310;
            this.Height = 170;
            MinimumSize = new Size(310, 170);
        }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (ValidateChildren())
        {
            NamedAmount.Amount = Math.Round(double.Parse(txtAmount.Text), 2);
            NamedAmount.Category = string.IsNullOrWhiteSpace(cbCategory.Text) ? category : cbCategory.Text.TrimEnd('s');
            NamedAmount.AmountName = txtName.Text.Trim();
            
            DialogResult = DialogResult.OK;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void txtAmount_Validating(object sender, CancelEventArgs e)
    {
        errorProvider.SetError(txtAmount, string.Empty);
        double amount;
        if (!txtAmount.Text.Contains('.'))
        {
            txtAmount.AppendText(".00");
        }
        if (txtAmount.Text.Length == 0)
        {
            e.Cancel = true;
            errorProvider.SetError(txtAmount, "Specify amount.");
        }
        else if (!double.TryParse(txtAmount.Text, out amount))
        {
            e.Cancel = true;
            errorProvider.SetError(txtAmount, "Must be numeric.");
        }
        else if (amount < 0)
        {
            e.Cancel = true;
            errorProvider.SetError(txtAmount, "Must be non-negative.");
        }
    }

    private void txtName_Validating(object sender, CancelEventArgs e)
    {
        errorProvider.SetError(txtName, string.Empty);
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            e.Cancel = true;
            errorProvider.SetError(txtName, "Enter source name.");
        }
    }
}
