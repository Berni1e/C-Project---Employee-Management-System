using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MG_Project.DataAccess.Database.Repositories
{
    // To jest klasa-matka dla wszystkich repozytoriów.
    // To ona udostępnia właściwość "Entities", której Ci brakowało.
    public abstract class RepositoryDatabaseBase<T> where T : class
    {
        protected readonly MGProjectDbContext _db;
        protected DbSet<T> Entities => _db.Set<T>();

        protected RepositoryDatabaseBase(MGProjectDbContext db)
        {
            _db = db;
        }

        public virtual IQueryable<T> Query()
            => Entities.AsQueryable();

        public virtual T? Get(Guid id)
        {
            return Entities.Find(id);
        }

        public virtual void Add(T entity)
            => Entities.Add(entity);

        public virtual void Remove(T entity)
            => Entities.Remove(entity);
    }
}