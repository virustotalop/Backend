using EFModeling.EntityProperties.FluentAPI.Required;
using Microsoft.EntityFrameworkCore.Storage;
using static Transaction;

public class TransactionService
{
    private readonly ITransactionRepository _repo;
    private readonly BackendDbContext _context;

    public TransactionService(ITransactionRepository repo, BackendDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    private decimal GetDebitCreditAffectedAmount(DebitOrCredit debitOrCredit, decimal amount)
    {
        return debitOrCredit.Equals(DebitOrCredit.Debit) ? amount * -1 : amount;
    }

    private DebitOrCredit ParseDebitCreditFromString(string debitOrCreditStr)
    {
        try
        {
            return (DebitOrCredit)Enum.Parse(typeof(DebitOrCredit), debitOrCreditStr, true);
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Cannot parse debit or credit string");
        }
    }

    public async Task<Transaction?> CreateTransactionAsync(int accountId, decimal amount, string debitOrCreditStr, string description)
    {
        DebitOrCredit debitOrCredit = ParseDebitCreditFromString(debitOrCreditStr);
        using IDbContextTransaction transactionScope = await _context.Database.BeginTransactionAsync();
        Transaction transaction = new Transaction
        {
            Amount = amount,
            DebitCredit = debitOrCredit,
            Description = description,
            AccountId = accountId,
        };

        Account? account = await _context.Account.FindAsync(accountId);
        if (account == null)
        {
            throw new InvalidOperationException("Cannot create a transaction for an account that does not exist");
        }

        decimal balanceAffectedAmount = GetDebitCreditAffectedAmount(debitOrCredit, amount);
        account.CurrentBalance += balanceAffectedAmount;
        
        //Save changes first to see if balance goes negative, if it does it should throw an exception
        await _repo.SaveChangesAsync();
        await _repo.AddAsync(transaction);
        await _repo.SaveChangesAsync();
        await transactionScope.CommitAsync();
        return transaction;
    }

    public async Task<List<Transaction>> GetTransactionsForAccountAsync(int accountId)
    {
        Account? account = await _context.Account.FindAsync(accountId);
        if (account == null)
        {
            throw new InvalidOperationException("Cannot get transactions for an account that does not exist");
        }
        return await _repo.GetByAccountIdAsync(accountId);
    }
        

    public async Task<bool> UpdateTransactionAsync(int transactionId, decimal newAmount, string newDebitOrCreditStr, string newDescription)
    {
        DebitOrCredit newDebitOrCredit = ParseDebitCreditFromString(newDebitOrCreditStr);

        Transaction? transaction = await _repo.GetByIdAsync(transactionId);
        if (transaction == null)
        {
            throw new InvalidOperationException("Cannot update a non-existent transaction");
        }

        using IDbContextTransaction transactionScope = await _context.Database.BeginTransactionAsync();

        Account? account = await _context.Account.FindAsync(transaction.AccountId);
        if (account == null)
        {
            throw new InvalidOperationException("Cannot create a transaction for an account that does not exist");
        }

        //Flip the value to restore the original value
        //Credit = +200 -> flip * -200
        //Debit = -200 -> flip * 200 
        account.CurrentBalance += GetDebitCreditAffectedAmount(transaction.DebitCredit, transaction.Amount) * -1;
        account.CurrentBalance += GetDebitCreditAffectedAmount(newDebitOrCredit, newAmount);

        transaction.Amount = newAmount;
        transaction.Description = newDescription;
        transaction.DebitCredit = newDebitOrCredit;

        await _repo.UpdateAsync(transaction);
        await _repo.SaveChangesAsync();
        await transactionScope.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteTransactionAsync(int transactionId)
    {
        Transaction? transaction = await _repo.GetByIdAsync(transactionId);
        if (transaction == null)
        {
            throw new InvalidOperationException("Cannot update a non-existent transaction");
        }

        using IDbContextTransaction transactionScope = await _context.Database.BeginTransactionAsync();

        Account? account = await _context.Account.FindAsync(transaction.AccountId);
        if (account == null)
        {
            throw new InvalidOperationException("Cannot create a transaction for an account that does not exist");
        }

        //Flip the value to restore the original value
        account.CurrentBalance += GetDebitCreditAffectedAmount(transaction.DebitCredit, transaction.Amount) * -1; 

        await _repo.DeleteAsync(transaction);
        await _repo.SaveChangesAsync();
        await transactionScope.CommitAsync();

        return true;
    }
}