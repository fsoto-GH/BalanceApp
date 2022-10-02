using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BalanceApp.App;

/// <summary>
/// Class used to display or save the transactions.
/// </summary>
internal static class TransactionPrinter
{
    private const int AMOUNT_MIN_SIZE = 7;


    private static string SectionHeader(string sectionTitle, int rowLength, char decorator = '~')
    {
        string topBorder = new(decorator, rowLength);
        string header = topBorder + "\r\n";
        header += string.Format("{1}{0}{1}\r\n", PadBoth(sectionTitle, rowLength - 2), decorator);
        header += topBorder + "\r\n\r\n";
        return header.ToString();
    }

    private static string PadBoth(string txt, int length)
    {
        if (txt.Length > length)
        {
            txt = txt.Substring(0, length);
        }
        length -= txt.Length;
        bool isEven = length % 2 == 0;
        length /= 2;
        return string.Format("{1}{0}{2}{1}", txt, new string(' ', length), isEven ? string.Empty : " ");
    }

    private static string SectionTotal(string totalLabel, double amount, int amountSpacing, int nameSpacing)
    {
        string _amount = string.Format("{0:0.00}", Math.Abs(amount));
        return string.Format(" {0}${1} [{2}]\r\n\r\n\r\n",
            amount < 0 ? "-" : " ",
            _amount.PadLeft(amountSpacing),
            totalLabel.PadRight(nameSpacing)
        );
    }

    private static string CategorySection(string category, TransactionTracker tracker, int rowLength, int amountSpacing, int nameSpacing)
    {
        var catItems = tracker.GetTransactionsOfType(category).OrderBy(x => x.AmountName).ToArray();
        if (catItems?.Any() == false)
            return string.Empty;

        StringBuilder summary = new();
        summary.Append(SectionHeader(category + "s", rowLength));
        for (int i = 0; i < catItems.Length; i++)
        {
            string amount = string.Format("{0:0.00}", catItems[i].Amount);
            summary.Append(string.Format("{0} ${1} [{2}]\r\n",
                i == catItems.Length - 1 && i > 0 ? "+" : " ",
                amount.PadLeft(amountSpacing),
                catItems[i].AmountName.PadRight(nameSpacing))
            );
        }
        summary.Append(new string('-', amountSpacing + 3) + "\r\n");
        summary.Append(SectionTotal($"Standing {category}s", tracker.SumOfType(category), amountSpacing, nameSpacing));
        return summary.ToString();
    } 

    public static string Summary(TransactionTracker tracker, char bannerChar = '█')
    {
        if (tracker is null)
            return string.Empty;

        var includedCategories = tracker.IncludedCategories;

        if (includedCategories.Any() == false)
            return string.Empty;
        
        StringBuilder summary = new();
        int nameMinSize = includedCategories.Any()? includedCategories.Max(cat => $"Standing {cat}s".Length): 0;
        int nameSpacing = Math.Max(tracker.GetMaxNameLength(), nameMinSize);
        int amountSpacing = Math.Max(tracker.GetMaxAmountLength(), AMOUNT_MIN_SIZE);

        // "  ${amountSpacing} [{nameSpacing}]".Length = rowLength
        int rowLength = 6 + amountSpacing + nameSpacing;

        if (tracker?.HasTransactions == true)
        {
            summary.Append(SectionHeader("Balance Sheet", rowLength, decorator: bannerChar));
        }

        foreach (string category in tracker.Categories)
        {
            summary.Append(CategorySection(category, tracker, rowLength, amountSpacing, nameSpacing));
        }

        if (tracker?.HasTransactions == true)
        {
            summary.Append(SectionHeader("Standing Totals", rowLength));
            summary.Append(SectionTotal("Standing Total", tracker.NetBalance, amountSpacing, nameSpacing));
        }

        return summary.ToString();
    }

    public static void ExportToFile(TransactionTracker tracker, string fileName)
    {
        using StreamWriter fileOut = new(fileName);
        foreach (DatedAmount t in tracker.Transactions.OrderBy(p => p.Category))
        {
            fileOut.WriteLine(string.Format("{0},{1},{2:0.00}", t.Category, t.AmountName, t.Amount));
        }
    }

    public static void SaveToFile(TransactionTracker tracker, string fileName)
    {
        using StreamWriter fileOut = new(fileName);
        fileOut.Write(Summary(tracker));
        fileOut.WriteLine(DateTime.Now.ToLongDateString());
    }
}
