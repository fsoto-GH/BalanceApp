using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BalanceApp.App;

/// <summary>
/// Class to encapsulate transactions and a means for interacting with them.
/// </summary>
public class TransactionTracker
{
    private readonly BindingList<DatedAmount> _transactions;
    private static readonly Dictionary<string, double> _catNetMult = new()
    {
        {"Balance", 1 },
        {"Payment", -1 },
        {"Cashback", 0 },
    };

    public string[] Categories { get; } = _catNetMult.Keys.ToArray();

    public TransactionTracker()
    {
        _transactions = new();
    }

    public List<DatedAmount> Transactions => _transactions.ToList();

    public bool HasTransactions => _transactions?.Any() == true;

    public double NetBalance => _transactions.Sum(x => x.Amount * _catNetMult[x.Category]);

    public void Add(DatedAmount datedAmount)
    {
        if (!Categories.Contains(datedAmount.Category))
        {
            throw new KeyNotFoundException($"Dated amount with category {datedAmount.Category} is invalid.");
        }
        _transactions.Add(datedAmount);
    }

    public void ClearTransactions()
    {
        _transactions.Clear();
    }

    public void Remove(DatedAmount datedAmount)
    {
        _transactions.Remove(datedAmount);
    }

    public List<DatedAmount> GetTransactionsOfType(string type)
    {
        if (!Categories.Contains(type))
        {
            throw new InvalidEnumArgumentException($"The type '{type}' is invalid.");
        }

        return _transactions.Where(x => string.Equals(x.Category, type)).ToList();
    }

    public bool HasAnyOfType(string type)
    {
        return _transactions.Any(x => string.Equals(x.Category, type)) == true;
    }

    public double SumOfType(string type)
    {
        return _transactions.Where(x => string.Equals(x.Category, type)).Sum(x => x.Amount);
    }

    public int GetMaxNameLength()
    {
        if (!_transactions.Any())
            return 0;
        return _transactions.Max(x => x.AmountName.Length);
    }

    public int GetMaxAmountLength()
    {
        if (!_transactions.Any())
            return 0;
        return _transactions.Max(x => $"{x.Amount:0.00}".Length);
    }

    /// <summary>
    /// This yields a set including only the categories which have at least on existing transaction.
    /// Example: suppose categories A, B, and C exist, but there is only one transaction of type C.
    /// Then this will return a set only containing C.
    /// </summary>
    public HashSet<string> IncludedCategories => _transactions.Select(x => x.Category).ToHashSet();
}
