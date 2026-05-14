using MG_Project.Abstractions;
using MG_Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataAccess.Memory.Repositories
{
    public class FirmRepositoryMemory : IFirmRepositories
    {
        private readonly MemoryDbContext _db;
        public FirmRepositoryMemory(MemoryDbContext db) => _db = db;

        public IQueryable<Firm> Query() => _db.Firm.AsQueryable();

        public Firm? Get(Guid id) => _db.Firm.FirstOrDefault(f => f.FirmId == id);

        public void Add(Firm entity) => _db.Firm.Add(entity);

        public void Remove(Firm entity) => _db.Firm.Remove(entity);
    }
}