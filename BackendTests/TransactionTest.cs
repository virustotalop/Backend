using Azure.Core;
using EFModeling.EntityProperties.FluentAPI.Required;
using Microsoft.EntityFrameworkCore.Storage;

[TestClass]
public class TransactionsTest
{

    private static BackendDbContext? _context;
    private static Task? _webAppTask;
    private static HttpClient? _client;

    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        _context = new BackendDbContext();
        WebApplication app = Program.CreateWebApplication(_context);
        _webAppTask = app.RunAsync();
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
    }

    [TestMethod]
    public async Task GetTransactions()
    {
        Transaction transaction = await _client.GetFromJsonAsync<Transaction>("/api/account/1/transaction");
        Assert.IsNotNull(transaction);
    }

    [TestMethod]
    public async Task CreateTransaction()
    {

        using IDbContextTransaction dbTransaction = await _context.Database.BeginTransactionAsync();

        Transaction request = new Transaction
        {
            Amount = 100,
            DebitCredit = Transaction.DebitOrCredit.Credit,
            Description = "DB Test",
            AccountId = 1
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/account/1/transaction", request);
        response.EnsureSuccessStatusCode();

        Transaction? transaction = await response.Content.ReadFromJsonAsync<Transaction>();
        Assert.Equals(100, transaction!.Amount);
        Assert.Equals("DB Test", transaction.Description);

        await dbTransaction.RollbackAsync();
    }

    [TestMethod]
    public async Task DeleteTransaction()
    {

        using IDbContextTransaction dbTransaction = await _context.Database.BeginTransactionAsync();

        Transaction request = new Transaction
        {
            Amount = 100,
            DebitCredit = Transaction.DebitOrCredit.Credit,
            Description = "DB Test",
            AccountId = 1
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/account/1/transaction", request);
        response.EnsureSuccessStatusCode();

        Transaction? transaction = await response.Content.ReadFromJsonAsync<Transaction>();

        HttpResponseMessage deleteResponse = await _client.DeleteAsync("/api/account/1/transaction/" + transaction.Id);
        deleteResponse.EnsureSuccessStatusCode();

        await dbTransaction.RollbackAsync();
    }

    [TestMethod]
    public async Task UpdateTransaction()
    {

        using IDbContextTransaction dbTransaction = await _context.Database.BeginTransactionAsync();

        Transaction request = new Transaction
        {
            Amount = 100,
            DebitCredit = Transaction.DebitOrCredit.Credit,
            Description = "DB Test",
            AccountId = 1
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/account/1/transaction", request);
        response.EnsureSuccessStatusCode();

        Transaction? transaction = await response.Content.ReadFromJsonAsync<Transaction>();

        Transaction updatedRequest = new Transaction
        {
            Amount = 200,
            DebitCredit = Transaction.DebitOrCredit.Credit,
            Description = "DB Test",
            AccountId = 1
        };

        HttpResponseMessage deleteResponse = await _client.PutAsync("/api/account/1/transaction/" + transaction.Id, updatedRequest);
        deleteResponse.EnsureSuccessStatusCode();

        await dbTransaction.RollbackAsync();
    }
}