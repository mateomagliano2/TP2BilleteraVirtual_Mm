using BilleteraVirtualAPI_Mm.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraVirtualAPI_Mm.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<TransaccionBV> TransaccionBVs { get; set; }
    }
}
