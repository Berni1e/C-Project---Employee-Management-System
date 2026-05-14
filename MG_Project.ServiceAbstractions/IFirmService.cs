using MG_Project.DataModel;
using System;
using System.Collections.Generic;

namespace MG_Project.ServiceAbstractions
{
    public interface IFirmService
    {
        Guid AddFirm(Guid firmId, string name, Address address, List<Department> depList);

        bool Remove(Guid firmId);

        Firm? Get(Guid id);
        IReadOnlyList<Department> GetAllDepartments(Guid firmId);
        IReadOnlyList<Employee> FindEmp(Guid firmId, string lastName);
        Employee? FindEmpById(Guid firmId, Guid empId);
        IReadOnlyList<(Firm, Department)> DispAllDep();
        IReadOnlyList<(Firm, Employee)> DispAllEmp();
        IReadOnlyList<Firm> GetAll();
    }
}