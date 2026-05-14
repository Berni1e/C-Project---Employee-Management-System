using MG_Project.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataAccess.Memory
{
    public class UnitOfWorkMemory : IUnitOfWork
    {
        private readonly MemoryDbContext _db;
        public UnitOfWorkMemory(MemoryDbContext db) => _db = db;
        public int SaveChanges() => _db.SaveChanges();
    }
}
