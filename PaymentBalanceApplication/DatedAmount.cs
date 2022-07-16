using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentBalanceApplication
{
    public class DatedAmount
    {
        public string AmountName { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }

        public DatedAmount() { }

        public DatedAmount(string name, double amount, string category)
        {
            AmountName = name;
            Amount = amount;
            Category = category;
        }

        public override string ToString()
        {
            return $"{AmountName} - {Amount:C2}";
        }
    }
}
