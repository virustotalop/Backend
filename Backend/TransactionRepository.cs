using EFModeling.EntityProperties.FluentAPI.Required;
using Microsoft.EntityFrameworkCore;
using System;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(int id);
    Task<List<Transaction>> GetByAccountIdAsync(int accountId);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(Transaction transaction);
    Task SaveChangesAsync();
}

public class TransactionRepository : ITransactionRepository
{
    private readonly BackendDbContext _context;
    public TransactionRepository(BackendDbContext context) => _context = context;

    public async Task<Transaction?> GetByIdAsync(int id) =>
        await _context.Transaction.FindAsync(id);

    public async Task<List<Transaction>> GetByAccountIdAsync(int accountId) =>
        await _context.Transaction
            .Where(t => t.AccountId == accountId)
            .ToListAsync();

    public async Task AddAsync(Transaction transaction) =>
        await _context.Transaction.AddAsync(transaction);

    public Task UpdateAsync(Transaction transaction)
    {
        _context.Transaction.Update(transaction);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Transaction transaction)
    {
        _context.Transaction.Remove(transaction);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}