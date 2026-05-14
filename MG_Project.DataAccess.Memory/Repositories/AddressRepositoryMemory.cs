using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.Abstractions;
using MG_Project.DataModel;

namespace MG_Project.DataAccess.Memory.Repositories
{
    public class AddressRepositoryMemory : IAddressRepositories
    {
        private readonly MemoryDbContext _db;
        public AddressRepositoryMemory(MemoryDbContext db) => _db = db;

        public IQueryable<Address> Query() => _db.Address.AsQueryable();

        public Address? Get(Guid id) => _db.Address.FirstOrDefault(a => a.AddressId == id);

        public void Add(Address entity) => _db.Address.Add(entity);
        public void Remove(Address entity) => _db.Address.Remove(entity);

    }
}
