using EFModeling.EntityProperties.FluentAPI.Required;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Text.Json;


public class Program
{
    private const string AccountJsonKey = "accounts";
    private const string TransactionJsonKey = "transactions";

    public static void Main(string[] args)
    {
        using (BackendDbContext context = new BackendDbContext())
        {
            WebApplication application = CreateWebApplication(context);
            application.Run();
        }
    }

    public static WebApplication CreateWebApplication(BackendDbContext context)
    {
         context.Database.EnsureCreated();
        SeedDatabase(context);

        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.Services.AddSingleton(context);
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<TransactionService>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<AccountService>();
        builder.Services.AddControllers()
        .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TransactionsController).Assembly));
        builder.Services.AddControllers()
        .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AccountController).Assembly));

        WebApplication app = builder.Build();
        app.MapControllers();
        return app; 
    }

    private static void SeedDatabase(BackendDbContext context)
    {
        string readJsonString = File.ReadAllText("Data.json");
        JsonDocument doc = JsonDocument.Parse(readJsonString);
        JsonElement docRoot = doc.RootElement;

        //Seed account data if table is empty
        if (!context.Account.Any())
        {
            List<Account> accountList = docRoot.GetProperty(AccountJsonKey).Deserialize<List<Account>>();
            context.Account.AddRange(accountList);
        }

        //Seed transaction data if table is empty
        if (!context.Transaction.Any())
        {
            List<Transaction> transactionList = docRoot.GetProperty(TransactionJsonKey).Deserialize<List<Transaction>>();
            foreach (Transaction transaction in transactionList)
            {
                Console.WriteLine(transaction.ToString());
            }
            context.Transaction.AddRange(transactionList);
        }

        context.SaveChanges();
    }
}
