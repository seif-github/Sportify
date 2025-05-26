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
            var connectionString = "Server=db20303.public.databaseasp.net; Database=db20303; User Id=db20303; Password=2Ze+d#S6@K5a; Encrypt=False; MultipleActiveResultSets=True;"; 
            //var connectionString = "Data Source=.; Initial Catalog=SportifyV1; Integrated Security=True; Encrypt=False; Trust Server Certificate=True"; 
            //var connectionString = "Data Source=SQL1001.site4now.net;Initial Catalog=db_ab9745_sportifyv1;User Id=db_ab9745_sportifyv1_admin;Password=sportify123"; 
            optionsBuilder.UseSqlServer(connectionString);

            return new SportifyContext(optionsBuilder.Options);
        }
    }
}
