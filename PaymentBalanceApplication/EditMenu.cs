using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaymentBalanceApplication
{
    public partial class EditMenu : Form
    {
        private BindingList<DatedAmount> transactions;
        private App parent;

        public EditMenu(BindingList<DatedAmount> trns, App parent)
        {
            InitializeComponent();
            Text = "Edit Menu";
            transactions = trns;

            resetDropDowns();
            this.parent = parent;
        }

        private void resetDropDowns()
        {
            cbCategory.Items.Clear();
            if (transactions.Any(t => t.Category == "Balance"))
            {
                cbCategory.Items.Add("Balances");
            }
            if (transactions.Any(t => t.Category == "Payment"))
            {
                cbCategory.Items.Add("Payments");
            }
            if (transactions.Any(t => t.Category == "Cashback"))
            {
                cbCategory.Items.Add("Cashbacks");
            }

            if (cbCategory.Items.Count > 0)
            {
                cbCategory.SelectedIndex = 0;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            transactions.Remove((DatedAmount)cbSelect.SelectedItem);
            resetDropDowns();
            if (!parent.hasItems())
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
                cbCategory.Enabled = false;
                cbSelect.Enabled = false;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddEditNamedAmount edit = new AddEditNamedAmount((DatedAmount)cbSelect.SelectedItem, cbCategory.Text, "Make Edit", true);
            DialogResult result = edit.ShowDialog();
            string selCat = cbCategory.Text;
            if(result == DialogResult.OK)
            {
                cbSelect.SelectedItem = edit.NamedAmount;
            }

            resetDropDowns();
            if (!transactions.Any(t => t.Category == selCat.TrimEnd('s')))
            {
                cbCategory.SelectedIndex = 0;
            } else
            {
                cbCategory.SelectedIndex = cbCategory.FindStringExact(selCat);
            }

        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbCategory.Text == "Balances")
            {
                cbSelect.DataSource = transactions.Where(t => t.Category == "Balance").OrderBy(t => t.AmountName).ToList();
            } else if (cbCategory.Text == "Payments")
            {
                cbSelect.DataSource = transactions.Where(t => t.Category == "Payment").OrderBy(t => t.AmountName).ToList();
            }
            else if (cbCategory.Text == "Cashbacks")
            {
                cbSelect.DataSource = transactions.Where(t => t.Category == "Cashback").OrderBy(t => t.AmountName).ToList();
            }
        }

        private void Edit_Load(object sender, EventArgs e)
        {
            cbCategory.SelectedIndex = 0;
        }
    }
}
