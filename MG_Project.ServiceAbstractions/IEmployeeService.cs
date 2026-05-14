using MG_Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.ServiceAbstractions
{
    public interface IEmployeeService
    {
        Employee AddEmployee(
        Guid empId,
        string firstName,
        string lastName,
        string pesel,
        DateTime dateOfBirth,
        Address address,
        string position,
        double baseSalary,
        double bonus,
        DateTime hireDate,
        List<Tasks> taskList,
        Guid? departmentId = null,
        Guid? firmId = null
    );


        Employee? get(Guid id);
        bool RemoveEmployee(Guid id);
        IReadOnlyList<Employee> CalcSalary();
        IReadOnlyList<Employee> FullInfo();
        IReadOnlyList<Tasks> ShowAllTasks();
    }
}
