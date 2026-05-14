using MG_Project.Abstractions;

namespace MG_Project.DataAccess.Database
{
    public class UnitOfWorkDatabase : IUnitOfWork
    {
        private readonly MGProjectDbContext _db;

        public UnitOfWorkDatabase(MGProjectDbContext db)
        {
            _db = db;
        }

        // Zmiana z void na int i dodanie return
        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
    }
}