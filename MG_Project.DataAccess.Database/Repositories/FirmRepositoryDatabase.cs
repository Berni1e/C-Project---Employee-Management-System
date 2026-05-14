using Microsoft.EntityFrameworkCore;
using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using MG_Project.Abstractions;
using System;
using System.Linq;

namespace MG_Project.DataAccess.Database.Repositories
{
    public class FirmRepositoryDatabase : RepositoryDatabaseBase<Firm>, IFirmRepositories
    {
        public FirmRepositoryDatabase(MGProjectDbContext db) : base(db) { }

        public override Firm? Get(Guid id)
        {
            return Entities
                .Include(f => f.Address)
                .Include(f => f.DepList)
                    .ThenInclude(d => d.EmpList)
                .Include(f => f.EmpList)
                .FirstOrDefault(f => f.FirmId == id);
        }

        public override IQueryable<Firm> Query()
        {
            return Entities
                .Include(f => f.Address)
                .Include(f => f.DepList)
                .Include(f => f.EmpList);
        }
    }
}