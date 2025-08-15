using EFModeling.EntityProperties.FluentAPI.Required;
public class AccountService
{
    private readonly IAccountRepository _repo;
    private readonly BackendDbContext _context;

    public AccountService(IAccountRepository repo, BackendDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<Account?> GetAccountAsync(int accountId)
    {
        return await _repo.GetAccountByIdAsync(accountId);
    }
}