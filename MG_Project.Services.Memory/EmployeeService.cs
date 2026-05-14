using MG_Project.Abstractions;
using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MG_Project.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepositories _employees;
        private readonly IPersonRepositories _persons;
        private readonly IDepartmentService _deptService;
        private readonly IFirmService _firmService;
        private readonly IUnitOfWork _uow;

        public EmployeeService(
            IEmployeeRepositories employees,
            IPersonRepositories persons,
            IDepartmentService deptService,
            IFirmService firmService,
            IUnitOfWork uow)
        {
            _employees = employees;
            _persons = persons;
            _deptService = deptService;
            _firmService = firmService;
            _uow = uow;
        }

        public Employee AddEmployee(
    Guid empId,
    string firstName,
    string lastName,
    string pesel,
    DateTime dateOfBirth,
    Address address,
    string position,
    double baseSalary,
    double bonus,
    DateTime hireDate,
    List<Tasks> taskList,
    Guid? departmentId = null,
    Guid? firmId = null)
        {
            // 1. Zapis osoby do Pamięci (To zostawiamy, bo PersonRepository jest Memory)
            var person = new Person
            {
                PersonId = empId,
                FirstName = firstName,
                LastName = lastName,
                Pesel = pesel,
                DateOfBirth = dateOfBirth,
                Address = address
            };
            _persons.Add(person);

            // 2. Tworzenie Pracownika (Do SQL)
            var emp = new Employee
            {
                EmployeeId = empId,
                FirstName = firstName,
                LastName = lastName,
                Pesel = pesel,
                DateOfBirth = dateOfBirth,
                Address = address,
                Position = position,
                BaseSalary = baseSalary,
                Bonus = bonus,
                HireDate = hireDate,
                TaskList = taskList ?? new List<Tasks>(),

                // --- POPRAWKA: Obsługa Firmy (SQL) ---
                // Firmy są w SQL, więc to możemy przypisać (o ile firma istnieje w bazie)
                FirmId = firmId,

                // --- POPRAWKA KLUCZOWA: Obsługa Działu (Memory -> SQL) ---
                // Ponieważ Działy nie istnieją w SQL, musimy wysłać NULL.
                // Inaczej SQL wyrzuci błąd "Foreign Key Constraint", bo nie znajdzie ID działu w swojej tabeli.
                DepartmentId = null
            };

            // Obsługa relacji obiektu Firmy (Jeśli Firmy są w SQL, to jest OK)
            if (firmId.HasValue)
            {
                // Tu pobieramy firmę. Jeśli FirmService korzysta z SQL, to zadziała.
                // Jeśli korzysta z Memory, to też może zadziałać, jeśli EF Core jest sprytny,
                // ale najbezpieczniej dla SQL jest upewnić się, że wysyłamy poprawne ID.
                var firm = _firmService.Get(firmId.Value);
                if (firm != null)
                {
                    emp.Firm = firm;
                }
            }

            // --- BLOKADA DZIAŁU ---
            // Ten fragment musimy WYŁĄCZYĆ (zakomentować), dopóki Działy nie będą w SQL.
            // Jeśli przypiszesz obiekt z pamięci do obiektu SQL, EF Core spróbuje go zapisać/powiązać
            // i wywali błąd.

            /* if (departmentId.HasValue)
                emp.Department = _deptService.Get(departmentId.Value);
            */

            // Upewniamy się, że nawigacja też jest nullem
            emp.Department = null;

            // 3. Zapis do SQL
            _employees.Add(emp);
            _uow.SaveChanges();

            return emp;
        }

        public bool RemoveEmployee(Guid id)
        {
            var emp = _employees.Get(id);
            if (emp == null) return false;

            _employees.Remove(emp);
            _uow.SaveChanges();
            return true;
        }

        public Employee? get(Guid id)
        {
            return _employees.Get(id);
        }

        public IReadOnlyList<Employee> CalcSalary()
        {
            var employees = _employees.Query().ToList();

            foreach (var e in employees)
                e.BaseSalary += e.Bonus;

            _uow.SaveChanges();
            return new ReadOnlyCollection<Employee>(employees);
        }

        public IReadOnlyList<Employee> FullInfo()
        {
            return new ReadOnlyCollection<Employee>(_employees.Query().ToList());
        }

        public IReadOnlyList<Tasks> ShowAllTasks()
        {
            return new ReadOnlyCollection<Tasks>(_employees.Query()
                .SelectMany(e => e.TaskList)
                .ToList());
        }

        public bool EditEmployee(
    Guid id,
    string firstName,
    string lastName,
    string pesel,
    string position,
    double salary,
    Guid? newFirmId) // <--- Nowy parametr: ID nowej firmy
        {
            // 1. Pobieramy pracownika z bazy (Repozytorium SQL pobierze go od razu z trackingiem)
            var emp = _employees.Get(id);
            if (emp == null) return false;

            // 2. Aktualizujemy proste pola
            emp.FirstName = firstName;
            emp.LastName = lastName;
            emp.Pesel = pesel;
            emp.Position = position;
            emp.BaseSalary = salary;

            // 3. Aktualizacja Firmy (Kluczowe dla Ciebie)
            // Jeśli ID się zmieniło, EF Core sam ogarnie zmianę relacji po ID
            if (emp.FirmId != newFirmId)
            {
                emp.FirmId = newFirmId;

                // Opcjonalnie: Jeśli newFirmId jest null, czyścimy obiekt
                if (newFirmId == null)
                {
                    emp.Firm = null;
                }
                // Jeśli nie jest null, EF może wymagać pobrania obiektu firmy, 
                // ale zazwyczaj samo zmiana ID wystarczy w EF Core.
            }

            // 4. Zapisujemy zmiany (SQL UPDATE)
            _uow.SaveChanges();
            return true;
        }
    }
}
