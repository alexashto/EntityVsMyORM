using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyORM;

namespace ORMComparsion.Domains
{
    [Table("Companies")]
    class Company
    {
        [Field("CompanyId", "int")]
        public int CompanyId { get; set; }
        [Field("Name", "string")]
        public string Name { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}
