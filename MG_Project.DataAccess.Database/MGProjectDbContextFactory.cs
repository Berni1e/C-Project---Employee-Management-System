using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MG_Project.DataAccess.Database
{
    // Ta klasa służy TYLKO do tego, żeby komendy Add-Migration/Update-Database wiedziały co robić.
    public class MGProjectDbContextFactory : IDesignTimeDbContextFactory<MGProjectDbContext>
    {
        public MGProjectDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MGProjectDbContext>();

            // Konfiguracja połączenia SQL
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MG_Project_Db;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new MGProjectDbContext(optionsBuilder.Options);
        }
    }
}
