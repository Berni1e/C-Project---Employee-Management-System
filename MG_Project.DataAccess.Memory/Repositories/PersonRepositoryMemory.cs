using MG_Project.Abstractions;
using MG_Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataAccess.Memory.Repositories
{
    public class PersonRepositoryMemory : IPersonRepositories
    {
        private readonly MemoryDbContext _db;
        public PersonRepositoryMemory(MemoryDbContext db) => _db = db;
        public IQueryable<Person> Query() => _db.Person.AsQueryable();
        public Person? Get(Guid id) => _db.Person.FirstOrDefault(p => p.PersonId == id);
        public void Add(Person entity) => _db.Person.Add(entity);
        public void Remove(Person entity) => _db.Person.Remove(entity);
    }
}
