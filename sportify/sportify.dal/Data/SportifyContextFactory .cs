using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Data
{
    public class SportifyContextFactory : IDesignTimeDbContextFactory<SportifyContext>
    {
        public SportifyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SportifyContext>();
            var connectionString = "Data Source=.; Initial Catalog=SportifyDB; Integrated Security=True; Encrypt=False; Trust Server Certificate=True"; 
            optionsBuilder.UseSqlServer(connectionString);

            return new SportifyContext(optionsBuilder.Options);
        }
    }
}
