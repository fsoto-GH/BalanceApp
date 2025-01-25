using BalanceApp.App;
using System.Collections.Generic;

namespace BalanceApp;

public class FileImportResult
{
    public int Additions { get; set; }
    public int TotalLines { get; set; }
    public List<DatedAmount> ReadTransactions { get; set; } = new();
}
