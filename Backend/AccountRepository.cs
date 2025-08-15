
using EFModeling.EntityProperties.FluentAPI.Required;

public interface IAccountRepository
{
    Task<Account?> GetAccountByIdAsync(int accountId);

}

public class AccountRepository : IAccountRepository
{
    private readonly BackendDbContext _context;
    public AccountRepository(BackendDbContext context) => _context = context;

    public async Task<Account?> GetAccountByIdAsync(int id) =>
        await _context.Account.FindAsync(id);

}