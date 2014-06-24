using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyORM;
using NUnit;
using NUnit.Framework;
using ORMComparsion.Domains;


namespace ORMComparsion
{
    [TestFixture]
    class TimeTests
    {
        MyAccessor<Employee> _employeesAccessor;
        MyAccessor<Company> _companiesAccessor;

        [TestFixtureSetUp]
        public void Init()
        {
            _employeesAccessor = new MyAccessor<Employee>();
            _companiesAccessor = new MyAccessor<Company>();
            using (var db = new CompaniesContext())
            {
                db.Database.Initialize(false);

                for (int i = 1; i <= 100; i++)
                {
                    var company = new Company();

                    var Employees = new List<Employee>()
                    {                      
                        new Employee() {Name = "Vitaly"},
                        new Employee() {Name = "Alex"},
                        new Employee() {Name = "Sara"},
                        new Employee() {Name = "Ekaterina"},
                        new Employee() {Name = "Bob"}
                    };

                    company.Name = String.Format("Company {0}", i);
                    company.Employees = Employees;

                    db.Companies.Add(company);
                }

                db.SaveChanges();
            }
        }


        [Test]
        public void SelectParentEntity()
        {
            for (int i = 0; i < 1000; i++)
            {
                using (var db = new CompaniesContext())
                {
                    var result = from company in db.Companies
                                 select company;

                }
            }
        }

        [Test]
        public void SelectParentAndChildEntity()
        {
            for (int i = 0; i < 1000; i++)
            {
                using (var db = new CompaniesContext())
                {
                    var result = from company in db.Companies
                                 from employee in db.Employees
                                 select new { CompanyName = company.Name, EmployeeName = employee.Name };

                }
            }
        }

        [Test]
        public void SelectParentMyORM()
        {
            for (int i = 0; i < 1000; i++)
            {
                var result = _companiesAccessor.GetAll();
            }
        }

        [Test]
        public void SelectParentAndChildMyORM()
        {
            for (int i = 0; i < 1000; i++)
            {
                var resultCompanies = _companiesAccessor.GetAll();
                var resultEmployees = _employeesAccessor.GetAll();
            }
        }



    }
}
