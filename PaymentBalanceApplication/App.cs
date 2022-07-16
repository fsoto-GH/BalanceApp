using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PaymentBalanceApplication
{
    public partial class App : Form
    {
        private BindingList<DatedAmount> transactions;
        private int amountSpacing;
        private int nameSpacing;
        private string[] Categories = new string[] { "Balance", "Payment", "Cashback" };

        public App()
        {
            InitializeComponent();
            transactions = new BindingList<DatedAmount>();
            amountSpacing = 0;
            nameSpacing = 17;
        }

        private void showAddDatedAmountDialog(string cat, string mode)
        {
            AddEditNamedAmount addBalance = new AddEditNamedAmount(cat, mode);
            DialogResult result = addBalance.ShowDialog();
            if (result == DialogResult.OK)
            {
                DatedAmount namedAmount = addBalance.NamedAmount;
                transactions.Add(namedAmount);

                UpdateSummary();
            }
        }

        private void btnAddBalance_Click(object sender, EventArgs e)
        {
            showAddDatedAmountDialog("Balance", "Add");
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            showAddDatedAmountDialog("Payment", "Add");

        }

        private void btnAddCashback_Click(object sender, EventArgs e)
        {
            showAddDatedAmountDialog("Cashback", "Add");
        }

        private void UpdateSummary()
        {
            int amountSpacing = 0, nameSpacing = 17;

            if (hasItems())
            {
                amountSpacing = transactions.Max(p => string.Format("{0:0.00}", p.Amount).Length);
                nameSpacing = transactions.Max(p => p.AmountName.Length);
            }

            this.amountSpacing = amountSpacing > this.amountSpacing ? amountSpacing : this.amountSpacing;
            this.nameSpacing = nameSpacing > this.nameSpacing ? nameSpacing : this.nameSpacing;

            txtSummary.Text = Summary();
            btnClear.Enabled = hasItems();
            btnEdit.Enabled = hasItems();
            btnExport.Enabled = hasItems();
            btnPrintTxt.Enabled = hasItems();
        }

        private void btnPrintTxt_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.FileName = string.Format("{0:00}-{1:00}-{2}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
            save.Filter = "txt|*.txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter fileOut = new StreamWriter(save.FileName))
                {
                    fileOut.Write(Summary());
                    fileOut.WriteLine(DateTime.Now.ToLongDateString());
                }
            }
        }

        private string Summary()
        {
            StringBuilder summary = new StringBuilder();
            int rowLength = nameSpacing + amountSpacing + 6;
            int priceLength = 3 + amountSpacing;

            if (hasItems())
            {
                summary.Append(SectionHeader("Balance Sheet", rowLength, '█'));
            }

            foreach (string cat in Categories)
            {
                summary.Append(printCat(cat, rowLength, priceLength));
            }

            if (hasItems())
            {
                summary.Append(SectionHeader("Standing Totals", rowLength));

                double net = transactions.Where(p => p.Category == "Balance" || p.Category == "Payment").Sum(p => p.Category == "Balance" ? p.Amount : -p.Amount);
                summary.Append(SectionTotal(net, "Total"));
            }

            return summary.ToString();
        }

        private string printCat(string category, int rowLength, int priceLength)
        {
            List<DatedAmount> catItems = transactions.Where(t => t.Category == category).OrderBy(t => t.AmountName).ToList();
            StringBuilder summary = new StringBuilder();

            if (catItems?.Any() == true)
            {
                double sum = 0;
                summary.Append(SectionHeader(category + "s", rowLength));
                for (int i = 0; i < catItems.Count; i++)
                {
                    string amount = string.Format("{0:0.00}", catItems[i].Amount);
                    summary.Append(string.Format("{2} ${0} [{1}]\r\n", amount.PadLeft(amountSpacing), catItems[i].AmountName.PadRight(nameSpacing), i == catItems.Count - 1 && i > 0 ? "+" : " "));
                    sum += Math.Round(catItems[i].Amount, 2);
                }
                summary.Append(new string('-', priceLength) + "\r\n");
                summary.Append(SectionTotal(sum, category));
            }

            return summary.ToString();
        }

        private string SectionHeader(string txt, int rowLength, char decorator = '~')
        {
            string topBorder = new string(decorator, rowLength);
            StringBuilder header = new StringBuilder();
            header.Append(topBorder + "\r\n");
            header.Append(string.Format("{1}{0}{1}\r\n", PadBoth(txt, rowLength - 2), decorator));
            header.Append(topBorder + "\r\n\r\n");
            return header.ToString();
        }

        private string SectionTotal(double total, string name)
        {
            string amount = string.Format("{0:0.00}", Math.Abs(total));
            return string.Format(" {2}${0} [Standing {1}]\r\n\r\n\r\n", amount.PadLeft(amountSpacing), name.PadRight(nameSpacing - 9), total < 0 ? "-" : " ");
        }

        private string PadBoth(string txt, int length)
        {
            bool odd = false;
            if (txt.Length > length)
            {
                txt = txt.Substring(0, length);
            }
            length -= txt.Length;
            if (length / 2 != ((double)length) / 2)
            {
                odd = true;
            }
            length /= 2;
            return string.Format("{1}{0}{2}{1}", txt, new String(' ', length), odd ? " " : "");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all?", "Clear All?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                transactions.Clear();
                UpdateSummary();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditMenu edit = new EditMenu(transactions, this);
            edit.ShowDialog();
            UpdateSummary();
        }

        public bool hasItems()
        {
            return transactions?.Any() == true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.FileName = string.Format("E_{0:00}-{1:00}-{2}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
            save.Filter = "txt|*.txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter fileOut = new StreamWriter(save.FileName))
                {
                    foreach (DatedAmount t in transactions.OrderBy(p => p.Category))
                    {
                        fileOut.WriteLine(string.Format("{0},{1},{2:0.00}", t.Category, t.AmountName, t.Amount));
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // ask user if they wish to delete any existing transactions
            if (hasItems())
            {
                DialogResult box = MessageBox.Show("There are existing transactions. Would you like to clear them?", "Clear Existing Transactions?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (box == DialogResult.Yes)
                {
                    transactions.Clear();
                }
                else if (box == DialogResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt";
            open.Filter = "txt|*.txt";

            int additions = 0;
            if (open.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(open.FileName);

                foreach (string line in lines)
                {
                    string[] comps = line.Split(',');

                    if (comps.Length == 3)
                    {
                        string cat = comps[0];
                        string name = comps[1];

                        if (double.TryParse(comps[2], out double amount) && Categories.Contains(cat))
                        {
                            additions++;
                            transactions.Add(new DatedAmount(name, amount, cat));
                        }
                    }
                }
                UpdateSummary();

                if (additions > 0)
                {
                    MessageBox.Show(string.Format("Successfully added {0}/{1} item(s)!", additions, lines.Length), "Success");
                }
                else
                {
                    MessageBox.Show("No additions made. Check if the file was empty or malformed.\nEach line should follow: \"[Balance|Payment|Cashback],Name,Amount\"");
                }
            }
        }
    }
}
