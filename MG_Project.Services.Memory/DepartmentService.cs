using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using MG_Project.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace MG_Project.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepositories _repo;
        private readonly IUnitOfWork _uow;

        public DepartmentService(IDepartmentRepositories repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Guid AddDep(Guid deptId, string deptName, List<Employee> empList)
        {
            var dep = new Department
            {
                DepartmentId = deptId,
                DeptName = deptName,
                EmpList = empList ?? new List<Employee>()
            };

            _repo.Add(dep);
            _uow.SaveChanges();

            return dep.DepartmentId;
        }

        public Department? Get(Guid id)
        {
            return _repo.Get(id);
        }

        public IReadOnlyList<Department> GetAllDepartments()
        {
            return _repo.Query().ToList().AsReadOnly();
        }

        public bool UpdateDep(Guid deptId, string newName)
        {
            var dep = _repo.Get(deptId);
            if (dep == null)
                return false;

            dep.DeptName = newName;
            _uow.SaveChanges();
            return true;
        }

        public bool RemoveDep(Guid id)
        {
            var dep = _repo.Get(id);
            if (dep == null)
                return false;

            _repo.Remove(dep);
            _uow.SaveChanges();
            return true;
        }

        public IReadOnlyList<Employee> AddEmp(Guid deptId, Employee emp)
        {
            var dep = _repo.Get(deptId);
            if (dep == null)
                return new List<Employee>().AsReadOnly();

            dep.AddEmp(emp);
            _uow.SaveChanges();

            return dep.EmpList.ToList().AsReadOnly();
        }

        public IReadOnlyList<Employee> DelEmp(Guid deptId, Employee emp)
        {
            var dep = _repo.Get(deptId);
            if (dep == null)
                return new List<Employee>().AsReadOnly();

            dep.DelEmp(emp);
            _uow.SaveChanges();

            return dep.EmpList.ToList().AsReadOnly();
        }
    }
}
