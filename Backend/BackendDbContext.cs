using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace EFModeling.EntityProperties.FluentAPI.Required;

public class BackendDbContext : DbContext
{
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Account> Account { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .Property(transaction => transaction.DebitCredit)
            .HasConversion(new ValueConverter<Transaction.DebitOrCredit, string>(
                v => v.ToString().ToLowerInvariant(),
                v => (Transaction.DebitOrCredit) Enum.Parse(typeof(Transaction.DebitOrCredit), v, true)
            )
        );
        modelBuilder.Entity<Account>()
            .ToTable(tb => tb.HasCheckConstraint(
                "CK_Account_CurrentBalanceMinimum", "[CurrentBalance] >= -[OverdraftLimit]"
            )
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? backendConnectionString = Environment.GetEnvironmentVariable("BACKEND_CONNECTION_STRING");
        optionsBuilder.UseSqlServer(backendConnectionString);

    }
}