using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ORMComparsion.Domains;

namespace ORMComparsion
{
    class CompaniesContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public CompaniesContext()
		{
            Database.SetInitializer(new DropCreateDatabaseAlways<CompaniesContext>());
		}
    }
}
