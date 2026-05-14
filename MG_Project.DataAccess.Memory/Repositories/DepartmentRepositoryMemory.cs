using MG_Project.Abstractions;
using MG_Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataAccess.Memory.Repositories
{
    public class DepartmentRepositoryMemory : IDepartmentRepositories
    {
        private readonly MemoryDbContext _db;
        public DepartmentRepositoryMemory(MemoryDbContext db) => _db = db;

        public IQueryable<Department> Query() => _db.Department.AsQueryable();

        public Department? Get(Guid id) => _db.Department.FirstOrDefault(d => d.DepartmentId == id);

        public void Add(Department entity) => _db.Department.Add(entity);

        public void Remove(Department entity) => _db.Department.Remove(entity);
    }
}