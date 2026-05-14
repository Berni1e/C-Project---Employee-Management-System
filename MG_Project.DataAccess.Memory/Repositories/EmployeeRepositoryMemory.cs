using MG_Project.Abstractions;
using MG_Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataAccess.Memory.Repositories
{
    public class EmployeeRepositoryMemory : IEmployeeRepositories
    {
        private readonly MemoryDbContext _db;
        public EmployeeRepositoryMemory(MemoryDbContext db) => _db = db;

        public IQueryable<Employee> Query() => _db.Employee.AsQueryable();

        public Employee? Get(Guid id) => _db.Employee.FirstOrDefault(p => p.EmployeeId == id);

        public void Add(Employee entity) => _db.Employee.Add(entity);

        public void Remove(Employee entity) => _db.Employee.Remove(entity);
    }
}