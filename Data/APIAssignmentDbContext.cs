
using APIAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAssignment.Data
{
    public class APIAssignmentDbContext: DbContext
    {
        public APIAssignmentDbContext(DbContextOptions options) : base(options) 
        {
        }
        public DbSet<Account>AccountsTable{ get; set; }
       

    }
}
