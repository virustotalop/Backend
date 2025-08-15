using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("api/account/{accountId}")]
public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAccount(int accountId)
    {
        try
        {
            var transactions = await _service.GetAccountAsync(accountId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}