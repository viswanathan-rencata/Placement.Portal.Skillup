using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Placement.Portal.Skillup.Models
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Salary { get; set; }
        public static List<Employee> GetList()
        {
            List<Employee> Employees = new List<Employee>{
          new Employee   { FirstName="Rahul", LastName="Kumar", Salary=45000},
          new Employee   { FirstName="Jose", LastName="Mathews", Salary=25000},
          new Employee   { FirstName="Ajith", LastName="Kumar", Salary=25000},
          new Employee   { FirstName="Scott", LastName="Allen", Salary=35000},
            new Employee   { FirstName="Abhishek", LastName="Nair", Salary=125000}
            };
            return Employees;
        }
    }
}
