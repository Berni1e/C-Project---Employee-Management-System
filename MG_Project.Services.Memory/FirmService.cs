using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using MG_Project.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace MG_Project.Services
{
    public class FirmService : IFirmService
    {
        private readonly IFirmRepositories _repo;
        private readonly IUnitOfWork _uow;

        public FirmService(IFirmRepositories repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Guid AddFirm(Guid firmId, string name, Address address, List<Department> depList)
        {
            var firm = new Firm
            {
                FirmId = firmId,
                Name = name,
                Address = address ?? new Address(),
                DepList = depList ?? new List<Department>()
            };

            _repo.Add(firm); //Zapis zmian
            _uow.SaveChanges();
            return firm.FirmId;
        }

        public bool Remove(Guid firmId)
        {
            var firm = _repo.Get(firmId);
            if (firm == null) return false;

            // KROK 1: Zrywanie zależności (Czyszczenie pracowników)

            if (firm.DepList != null)
            {
                foreach (var dept in firm.DepList)
                {
                    if (dept.EmpList != null)
                    {
                        // Tworzymy kopię listy, żeby móc modyfikować oryginał
                        var employeesToDetach = dept.EmpList.ToList();

                        foreach (var emp in employeesToDetach)
                        {
                            // Zwalniamy pracownika (zostaje w systemie, ale bez firmy)
                            emp.Firm = null;
                            emp.FirmId = null;
                            emp.Department = null;
                            emp.DepartmentId = null;
                        }
                    }
                }
            }

            _repo.Remove(firm);
            _uow.SaveChanges();

            return true;
        }

        public Firm? Get(Guid id)
        {
            return _repo.Get(id);
        }

        public IReadOnlyList<Department> GetAllDepartments(Guid firmId)
        {
            var firm = _repo.Get(firmId);
            return firm != null
                ? firm.DepList.AsReadOnly()
                : new ReadOnlyCollection<Department>(new List<Department>());
        }

        public IReadOnlyList<Employee> FindEmp(Guid firmId, string lastName)
        {
            var firm = _repo.Get(firmId);
            if (firm == null)
                return new ReadOnlyCollection<Employee>(new List<Employee>());

            var employees = firm.DepList
                .SelectMany(d => d.EmpList)
                .Where(e => e.LastName == lastName)
                .ToList();

            return new ReadOnlyCollection<Employee>(employees);
        }

        public Employee? FindEmpById(Guid firmId, Guid empId)
        {
            var firm = _repo.Get(firmId);
            if (firm == null)
                return null;

            return firm.DepList
                .SelectMany(d => d.EmpList)
                .FirstOrDefault(e => e.EmployeeId == empId);
        }

        public IReadOnlyList<(Firm, Department)> DispAllDep()
        {
            return _repo.Query()
                .ToList() // materializacja danych dla drzewa
                .SelectMany(firm => firm.DepList.Select(dep => (firm, dep)))
                .ToList();
        }

        public IReadOnlyList<(Firm, Employee)> DispAllEmp()
        {
            return _repo.Query()
                .ToList() // materializacja danych dla drzewa
                .SelectMany(firm => firm.DepList
                    .SelectMany(dep => dep.EmpList
                        .Select(emp => (firm, emp))))
                .ToList();
        }

        public IReadOnlyList<Firm> GetAll()
        {
            return _repo.Query().ToList();
        }
    }
}
