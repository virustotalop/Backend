using Azure.Core;
using EFModeling.EntityProperties.FluentAPI.Required;
using Microsoft.EntityFrameworkCore.Storage;

[TestClass]
public class TransactionsTest
{
    private static BackendDbContext? _context;
    private static Task? _webAppTask;
    private static HttpClient? _client;

    public TestContext? TestContext { get; set; }

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
        List<Transaction> transactions = await _client.GetFromJsonAsync<List<Transaction>>("/api/account/1/transaction");
        Assert.IsNotNull(transactions);
    }

    [TestMethod]
    public async Task CreateTransaction()
    {
        TransactionRequest request = new TransactionRequest
        {
            Amount = 100,
            DebitOrCredit = "credit",
            Description = "DB Test",
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/account/1/transaction", request);
        var body = await response.Content.ReadAsStringAsync();

        Transaction? transaction = await response.Content.ReadFromJsonAsync<Transaction>();
        Assert.IsNotNull(transaction);
        Assert.AreEqual(100, transaction!.Amount);
        Assert.AreEqual("DB Test", transaction.Description);
    }

    [TestMethod]
    public async Task DeleteTransaction()
    {
        TransactionRequest request = new TransactionRequest
        {
            Amount = 100,
            DebitOrCredit = "credit",
            Description = "DB Test"
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/account/1/transaction", request);
        response.EnsureSuccessStatusCode();

        Transaction? transaction = await response.Content.ReadFromJsonAsync<Transaction>();

        HttpResponseMessage deleteResponse = await _client.DeleteAsync("/api/account/1/transaction/" + transaction.Id);
        deleteResponse.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task UpdateTransaction()
    {
        TransactionRequest request = new TransactionRequest
        {
            Amount = 100,
            DebitOrCredit = "credit",
            Description = "DB Test",
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/account/1/transaction", request);
        response.EnsureSuccessStatusCode();

        Transaction? transaction = await response.Content.ReadFromJsonAsync<Transaction>();

        TransactionRequest updatedRequest = new TransactionRequest
        {
            Amount = 200,
            DebitOrCredit = "credit",
            Description = "DB Test",
        };

        HttpResponseMessage updateResponse = await _client.PutAsJsonAsync("/api/account/1/transaction/" + transaction.Id, updatedRequest);
        updateResponse.EnsureSuccessStatusCode();
    }
}