using MG_Project.DataAccess.Memory;
using MG_Project.DataAccess.Memory.Repositories;
using MG_Project.Abstractions;
using MG_Project.Services;
using MG_Project.ServiceAbstractions;
using MG_Project.DataModel;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG_Project.Test
{
    public class FirmServiceTests
    {
        private readonly IFirmService _firmService;
        private readonly IDepartmentService _departmentService;

        public FirmServiceTests()
        {
            var db = new MemoryDbContext();

            // Repozytoria
            IDepartmentRepositories deptRepo = new DepartmentRepositoryMemory(db);
            IFirmRepositories firmRepo = new FirmRepositoryMemory(db);

            IUnitOfWork uow = new UnitOfWorkMemory(db);

            // Serwisy
            _departmentService = new DepartmentService(deptRepo, uow);
            _firmService = new FirmService(firmRepo, uow);
        }

        // Metoda pomocnicza do tworzenia adresu dla Firmy
        private Address CreateFirmAddress()
        {
            return new Address
            {
                Street = "Corporate Blvd",
                City = "Warsaw",
                Country = "Polska",
                Zipcode = "00-001",
                HouseNumber = 100,
                ApartmentNumber = null
            };
        }

        private Employee CreateEmployee(string firstName, string lastName, string pesel, string position)
        {
            var address = new Address
            {
                Street = "Main Street",
                City = "Krakow",
                Country = "Polska",
                Zipcode = "12-345",
                HouseNumber = 10,
                ApartmentNumber = 1
            };

            return new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Pesel = pesel,
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = address,
                Position = position,
                BaseSalary = 5000,
                Bonus = 1000,
                HireDate = DateTime.Now.AddYears(-2)
            };
        }

        [Fact]
        public void AddFirm_ShouldCreateFirmWithDepartmentsAndAddress()
        {
            // Arrange
            var dept1 = new Department { DeptName = "Development" };
            var dept2 = new Department { DeptName = "Management" };
            var firmAddress = CreateFirmAddress();

            // Act
            // Poprawione wywołanie: dodano argument 'address'
            var firmId = _firmService.AddFirm(Guid.NewGuid(), "TechCorp", firmAddress, new List<Department> { dept1, dept2 });
            var firm = _firmService.Get(firmId);

            // Assert
            Assert.NotNull(firm);
            Assert.Equal("TechCorp", firm.Name);
            Assert.NotNull(firm.Address); // Sprawdzamy czy adres istnieje
            Assert.Equal("Warsaw", firm.Address.City);
            Assert.Equal(2, firm.DepList.Count);
            Assert.Contains(dept1, firm.DepList);
            Assert.Contains(dept2, firm.DepList);
        }

        [Fact]
        public void FindEmp_ShouldReturnEmployeeIfExists()
        {
            // Arrange
            var emp = CreateEmployee("John", "Doe", "12345678901", "Developer");
            var dept = new Department { DeptName = "Dev" };
            dept.AddEmp(emp);

            var firmAddress = CreateFirmAddress();

            // Act
            // Poprawione wywołanie: dodano argument 'address'
            var firmId = _firmService.AddFirm(Guid.NewGuid(), "TechCorp", firmAddress, new List<Department> { dept });
            var firm = _firmService.Get(firmId);

            var foundEmp = firm.FindEmp("12345678901");

            // Assert
            Assert.NotNull(foundEmp);
            Assert.Equal(emp.Pesel, foundEmp.Pesel);
            Assert.Equal(emp.FirstName, foundEmp.FirstName);
        }

        [Fact]
        public void FindEmp_ShouldReturnNullIfNotExists()
        {
            // Arrange
            var firmAddress = CreateFirmAddress();

            // Act
            // Poprawione wywołanie: dodano argument 'address' oraz pustą listę działów
            var firmId = _firmService.AddFirm(Guid.NewGuid(), "BizGroup", firmAddress, new List<Department>());
            var firm = _firmService.Get(firmId);

            var foundEmp = firm.FindEmp("00000000000");

            // Assert
            Assert.Null(foundEmp);
        }

        [Fact]
        public void AddDepartment_ShouldAddDepartmentToFirm()
        {
            // Arrange
            var firmAddress = CreateFirmAddress();

            // Poprawione wywołanie: dodano argument 'address'
            var firmId = _firmService.AddFirm(Guid.NewGuid(), "TechCorp", firmAddress, new List<Department>());
            var firm = _firmService.Get(firmId);

            var newDept = new Department { DeptName = "Support" };

            // Act
            firm.AddDep(newDept);

            // Assert
            Assert.Contains(newDept, firm.DepList);
        }
    }
}