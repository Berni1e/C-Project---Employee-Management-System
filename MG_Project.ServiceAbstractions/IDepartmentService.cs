using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.DataModel;

namespace MG_Project.ServiceAbstractions
{
    public interface IDepartmentService
    {
        Guid AddDep(Guid deptId, string deptName, List<Employee> empList);

        Department? Get(Guid id);
        IReadOnlyList<Department> GetAllDepartments();

        bool UpdateDep(Guid deptId, string newName);

        bool RemoveDep(Guid deptId);

        IReadOnlyList<Employee> AddEmp(Guid deptId, Employee emp);
        IReadOnlyList<Employee> DelEmp(Guid deptId, Employee emp);
    }
}
