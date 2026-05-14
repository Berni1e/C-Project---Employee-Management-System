using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.DataModel;
using Microsoft.EntityFrameworkCore;

namespace MG_Project.DataAccess.Database
{
    public class MGProjectDbContext : DbContext
    {
        public MGProjectDbContext(DbContextOptions<MGProjectDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Firm> Firms => Set<Firm>();
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Relacja Pracownik -> Firma
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Firm)
                .WithMany(f => f.EmpList)
                .HasForeignKey(e => e.FirmId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Relacja Pracownik -> Dział
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.EmpList)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Relacja Dział -> Firma
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Firm)
                .WithMany(f => f.DepList)
                .HasForeignKey(d => d.FirmId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
