using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new {Id = "1", Name = "Admin", NormilizeName = "ADMIN"},
                new {Id = "2", Name = "Customer", NormilizeName = "CUSTOMER"},
                new {Id = "3", Name = "Moderator", NormilizeName = "MODERATOR"}
            );       
        }
        public virtual DbSet<Value> Values { get; set; }
    }
}
