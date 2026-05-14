using Microsoft.EntityFrameworkCore;
using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using MG_Project.Abstractions;
using System;
using System.Linq;

namespace MG_Project.DataAccess.Database.Repositories
{
    public class EmployeeRepositoryDatabase : RepositoryDatabaseBase<Employee>, IEmployeeRepositories
    {
        public EmployeeRepositoryDatabase(MGProjectDbContext db) : base(db) { }

        // Nadpisujemy Get, aby pobierać pracownika RAZEM z Firmą i Działem (JOIN)
        public override Employee? Get(Guid id)
        {
            return Entities
                .Include(e => e.Firm)
                .Include(e => e.Department)
                .FirstOrDefault(e => e.EmployeeId == id);
        }

        // Nadpisujemy Query dla list, również z pełnymi danymi
        public override IQueryable<Employee> Query()
        {
            return Entities
                .Include(e => e.Firm)
                .Include(e => e.Department);
        }
    }
}