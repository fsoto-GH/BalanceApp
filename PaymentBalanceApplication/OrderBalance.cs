using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace PaymentBalanceApplication
{
    class OrderBalance { 

        public OrderBalance(double total)
        {
            this.Transactions = new BindingList<DatedAmount>();
            this.OrderItems = new BindingList<DatedAmount>();
        }

        public double Total
        {
            get
            {
                double tot = 0;
                foreach(DatedAmount x in Transactions)
                {
                    tot += x.Amount;
                }

                return tot;
            }
        }

        public BindingList<DatedAmount> Transactions { get; set; }

        public BindingList<DatedAmount> OrderItems { get; set; }
    }
}
