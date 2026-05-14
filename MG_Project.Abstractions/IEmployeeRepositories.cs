using System;
using System.Linq;
using MG_Project.DataModel;

namespace MG_Project.Abstractions
{
    public interface IEmployeeRepositories
    {
        IQueryable<Employee> Query();
        Employee? Get(Guid id);
        void Add(Employee entity);
        void Remove(Employee entity);
    }
}
