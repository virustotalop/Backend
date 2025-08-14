using EFModeling.EntityProperties.FluentAPI.Required;
using System.Text.Json;


class Program
{
    private const string AccountJsonKey = "accounts";
    private const string TransactionJsonKey = "transactions";

    public static void Main(string[] args)
    {
        using (BackendDbContext context = new BackendDbContext())
        {
            context.Database.EnsureCreated();
            SeedDatabase(context);
        }
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
