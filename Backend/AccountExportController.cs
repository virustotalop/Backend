
using EFModeling.EntityProperties.FluentAPI.Required;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

[ApiController]
[Route("api/export")]
public class ExportController : ControllerBase
{
    private readonly BackendDbContext _context;

    public ExportController(BackendDbContext context)
    {
        _context = context;
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> ExportAccountsCsv()
    {
        List<Account> accounts = await _context.Account.ToListAsync();
        List<Transaction> transactions = await _context.Transaction.ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Id,Name,CurrentBalance,OverdraftLimit");

        foreach (var account in accounts)
        {
                csv.AppendLine($"{account.Id}," +
                               $"{account.Name}," +
                               $"{account.CurrentBalance}," +
                               $"{account.OverdraftLimit}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "accounts.csv");
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> ExportTransactionsCsv()
    {
        List<Transaction> transactions = await _context.Transaction.ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Id,Description,DebitCredit,Amount,AccountId");

        foreach (var transaction in transactions)
        {
            csv.AppendLine($"{transaction.Id}," +
                           $"{transaction.Description}," +
                           $"{transaction.DebitCredit.ToString().ToLowerInvariant()}," +
                           $"{transaction.Amount}," +
                           $"{transaction.AccountId}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "transactions.csv");
    }
}