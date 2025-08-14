using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/account/{accountid}/transaction")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _service;

    public TransactionsController(TransactionService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetTransactions(int accountId)
    {
        try
        {
            var transactions = await _service.GetTransactionsForAccountAsync(accountId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(int accountId, [FromBody] TransactionRequest request)
    {
        try
        {
            Transaction? transaction = await _service.CreateTransactionAsync(
                request.AccountId, 
                request.Amount, 
                request.DebitOrCredit, 
                request.Description
            );
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{transactionId}")]
    public async Task<IActionResult> UpdateTransaction(int accountId, int transactionId, [FromBody] TransactionRequest request)
    {
        try
        {
            await _service.UpdateTransactionAsync(
                transactionId,
                request.Amount,
                request.DebitOrCredit,
                request.Description
            );
            return Ok("Transaction updated.");
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> DeleteTransaction(int accountId, int transactionId)
    {
        try
        {
            await _service.DeleteTransactionAsync(transactionId);
            return Ok("Transaction deleted.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class TransactionRequest
{
    public required decimal Amount { get; set; }

    public required string DebitOrCredit { get; set; }

    public required string Description { get; set; }

    public required int AccountId { get; set; }
}