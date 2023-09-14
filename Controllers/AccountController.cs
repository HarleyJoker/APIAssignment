using Microsoft.AspNetCore.Mvc;
using APIAssignment.Data;
using APIAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAssignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly APIAssignmentDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;

        public AccountController(APIAssignmentDbContext dbContext, ILogger<AccountController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Create a new account
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] Account accountRequest)
        {
            if (accountRequest == null)
            {
                return BadRequest("Account data must not be null");
            }

            // Add validation logic here if needed

            try
            {
                var account = CreateAccountFromRequest(accountRequest);
                await _dbContext.AccountsTable.AddAsync(account);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAccounts), new { id = account.ID }, account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating account");
                return BadRequest(ex.Message);
            }
        }

        // Helper method to create Account object
        private Account CreateAccountFromRequest(Account request)
        {
            return new Account
            {
                firstname = request.firstname,
                lastname = request.lastname,
                email = request.email,
                telephone = request.telephone,
                identitynumber = request.identitynumber
            };
        }

        // Create multiple accounts
        [HttpPost("CreateAccounts")]
        public async Task<IActionResult> CreateAccounts([FromBody] List<Account> accountsRequest)
        {
            if (accountsRequest == null || accountsRequest.Count == 0)
            {
                return BadRequest("Account list must not be null or empty");
            }

            try
            {
                await _dbContext.AccountsTable.AddRangeAsync(accountsRequest);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAccounts), accountsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating accounts");
                return BadRequest(ex.Message);
            }
        }

        // Get all accounts
        [HttpGet("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _dbContext.AccountsTable.ToListAsync();
            if (accounts == null || accounts.Count == 0)
            {
                return NotFound("No accounts found.");
            }
            return Ok(accounts);
        }

        // Update an existing account
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid id, [FromBody] Account updatedRequest)
        {
            if (updatedRequest == null)
            {
                return BadRequest("Updated data must not be null");
            }

            try
            {
                var account = await _dbContext.AccountsTable.FindAsync(id);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                account.firstname = updatedRequest.firstname;
                account.lastname = updatedRequest.lastname;
                account.email = updatedRequest.email;
                account.telephone = updatedRequest.telephone;
                account.identitynumber = updatedRequest.identitynumber;

                await _dbContext.SaveChangesAsync();
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating account");
                return BadRequest(ex.Message);
            }
        }

        // Delete an account
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            try
            {
                var account = await _dbContext.AccountsTable.FindAsync(id);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                _dbContext.AccountsTable.Remove(account);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting account");
                return BadRequest(ex.Message);
            }
        }
    }
}
