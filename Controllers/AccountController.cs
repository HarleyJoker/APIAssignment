using Microsoft.AspNetCore.Mvc;
using APIAssignment.Data;
using APIAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAssignment.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly APIAssignmentDbContext dbContext;

        public AccountController (APIAssignmentDbContext dbContext)
        {
            this.dbContext= dbContext;
        }

        [HttpPost]
        public  async Task<IActionResult> AddAccount(Account AddAccountRequest )
        {
            var account = new Account();
            account.firstname = AddAccountRequest.firstname;
            account.lastname = AddAccountRequest.lastname;
            account.email = AddAccountRequest.email;
            account.telephone = AddAccountRequest.telephone;
            account.identitynumber = AddAccountRequest.identitynumber;


            await dbContext.AccountsTable.AddAsync( account );

            await dbContext.SaveChangesAsync();



            return Ok(account);
        }
     
        [HttpPost]
        public async Task<IActionResult> AddAccounts(List<Account> AddAccountsRequest)
        {
            try
            {


                List<string> AddedAccountNames = new List<string>();
             
               

                foreach (var account in AddAccountsRequest)
                {
                    
                   
                    //account.ID = Guid.Empty;
                    //AddedAccountNames.Add($"{account.firstname}{account.lastname}");

                    await dbContext.AccountsTable.AddAsync(account);


                }



                await dbContext.SaveChangesAsync();

                return Ok(AddedAccountNames);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        
        public async  Task<IActionResult>ReturnAccounts()
        {

            return Ok(await dbContext.AccountsTable.ToListAsync());


        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>UpdateAccount([FromRoute] Guid id, Account ReturnUpdatedRequest)
        {
            var account = await dbContext.AccountsTable.FindAsync(id);

            if (account != null) {
                account.firstname = ReturnUpdatedRequest.firstname;
                account.lastname = ReturnUpdatedRequest.lastname;
                account.email = ReturnUpdatedRequest.email;
                account.telephone = ReturnUpdatedRequest.telephone;
                account.identitynumber = ReturnUpdatedRequest.identitynumber;

                await dbContext.SaveChangesAsync();

                return Ok(account); 
            }
            return NotFound(); 
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id, Account ReturnDeleteRequest)
        {
            var account = await dbContext.AccountsTable.FindAsync(id);

            if (account != null)
            {
                dbContext.Remove(account);
                await dbContext.SaveChangesAsync();
                return Ok(account);
            }
            return NotFound();
        }

    }
}
