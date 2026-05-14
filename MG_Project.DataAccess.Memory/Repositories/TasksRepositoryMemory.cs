using MG_Project.DataModel;
using MG_Project.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataAccess.Memory.Repositories
{
    public class TasksRepositoryMemory : ITasksRepositories
    {
        private readonly MemoryDbContext _db;
        public TasksRepositoryMemory(MemoryDbContext db) => _db = db;

        public IQueryable<Tasks> Query() => _db.Tasks.AsQueryable();

        public Tasks? Get(Guid id) => _db.Tasks.FirstOrDefault(t => t.TasksId == id);

        public void Add(Tasks entity) => _db.Tasks.Add(entity);

        public void Remove(Tasks entity) => _db.Tasks.Remove(entity);
    }
}