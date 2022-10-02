namespace PaymentTracker.App;

/// <summary>
/// Simplest representation of a transaction.
/// </summary>
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
