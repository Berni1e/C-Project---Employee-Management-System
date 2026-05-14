using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataModel
{
    using System;
    using System.Collections.Generic;

    public class Employee : Person
    {
        public Guid EmployeeId { get; set; }
        public string Position { get; set; }
        public double BaseSalary { get; set; }
        public double Bonus {  get; set; }
        public DateTime HireDate { get; set; }
        public List<Tasks> TaskList { get; set; } // kompozycja z Tasks
        public Guid? FirmId { get; set; }
        public Firm? Firm { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Employee()
        {
            EmployeeId = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            Pesel = string.Empty;
            DateOfBirth = DateTime.MinValue;
            Address = new Address();
            Position = string.Empty;
            BaseSalary = 0.0;
            Bonus = 0.0;
            HireDate = DateTime.MinValue;
            TaskList = new List<Tasks>();

            FirmId = null; // Zmień na null (chyba że ustawiasz to później)
            Firm = null;

            // --- TO JEST KLUCZOWA ZMIANA ---
            DepartmentId = null; // Było: Guid.NewGuid(); -> Musi być null!
            Department = null;   // Było: new Department(); -> Musi być null!
        }

        public Employee(string firstName, string lastName, string pesel, DateTime dateOfBirth, Address address,
                        string position, double baseSalary, double bonus, DateTime hireDate, Guid? firmId, Firm? firm, Guid? departmentId, Department? department)
            : base(firstName, lastName, pesel, dateOfBirth, address)
        {
            Position = position;
            BaseSalary = baseSalary;
            Bonus = bonus;
            HireDate = hireDate;
            FirmId = firmId;
            Firm = firm;
            DepartmentId = departmentId;
            Department = department;

            TaskList = new List<Tasks>();
        }

        public double CalcSalary()
        {
            return BaseSalary + Bonus;
        }

        public override string FullInfo()
        {
            return base.FullInfo() +
                   $"\nUnique ID given: {EmployeeId}\n" +
                   $"Position: {Position}\n" +
                   $"Hire Date: {HireDate:yyyy-MM-dd}\n" +
                   $"Salary: {CalcSalary():F2} PLN\n" +
                   $"Firm: {Firm.Name}, ID of firm: {FirmId}\n" +
                   $"Department: {Department.DeptName}, ID of department: {DepartmentId}\n" +
                   $"Tasks: {TaskList.Count}\n";
        }

        public void AddTask(Tasks task)
        {
            if (task != null)
            {
                TaskList.Add(task);
            }
        }

        public void ShowAllTasks()
        {
            Console.WriteLine($"\n--- Tasks for {FirstName} {LastName} ---");
            if (TaskList.Count == 0)
            {
                Console.WriteLine("No tasks assigned.");
            }
            else
            {
                foreach (var t in TaskList)
                {
                    t.ShowTask();
                }
            }
        }
    }


}
