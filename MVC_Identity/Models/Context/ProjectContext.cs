using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_Identity.Models.Entity;

namespace MVC_Identity.Models.Context
{
    public class ProjectContext:IdentityDbContext<AppUser>
    {





        public ProjectContext(DbContextOptions<ProjectContext> options):base(options)
        {
            
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("server=DESKTOP-J4PTH70;database=IdentitySampleDB;uid=sa;pwd=123;TrustServerCertificate=True");
        //    }

        //    base.OnConfiguring(optionsBuilder);
        //}

    }
}
