using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Firm
    {
        public Guid FirmId { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public List<Department> DepList { get; set; }

        public Firm()
        {
            FirmId = Guid.NewGuid();
            Name = string.Empty;
            Address = new Address();
            DepList = new List<Department>();
        }
        public Firm(string name, Address address)
        {
            FirmId = Guid.NewGuid();
            Name = name;
            Address = address;
            DepList = new List<Department>();
        }

        public virtual ICollection<Employee> EmpList { get; set; } = new List<Employee>();

        public Department AddDep(Department dep)
        {
            if (dep != null && !DepList.Contains(dep))
            {
                DepList.Add(dep);
            }
            return dep;
        }

        public Employee FindEmp(string pesel)
        {
            foreach (var dep in DepList)
            {
                var emp = dep.EmpList.FirstOrDefault(e => e.Pesel == pesel);
                if (emp != null)
                    return emp;
            }
            return null; // nie znaleziono pracownika
        }

        public void DispAllDep()
        {
            Console.WriteLine($"--- Departments in {Name} ---");

            if (DepList.Count == 0)
            {
                Console.WriteLine("No departments found.");
            }
            else
            {
                foreach (var dep in DepList)
                {
                    Console.WriteLine($"Department ID: {dep.DepartmentId}, Name: {dep.DeptName}, Employees: {dep.EmpList.Count}");
                }
            }

            Console.WriteLine();
        }

        public void DispAllEmp()
        {
            Console.WriteLine($"--- Employees in {Name} ---");

            if (DepList.Count == 0)
            {
                Console.WriteLine("No departments to display employees from.");
            }
            else
            {
                foreach (var dep in DepList)
                {
                    Console.WriteLine($"\nDepartment: {dep.DeptName}");
                    if (dep.EmpList.Count == 0)
                    {
                        Console.WriteLine("  No employees in this department.");
                    }
                    else
                    {
                        foreach (var emp in dep.EmpList)
                        {
                            Console.WriteLine($"  - {emp.FirstName} {emp.LastName}, PESEL: {emp.Pesel}, Position: {emp.Position}");
                        }
                    }
                }
            }

            Console.WriteLine();
        }
    }

}
