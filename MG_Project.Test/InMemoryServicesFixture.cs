using MG_Project.DataAccess.Memory;
using MG_Project.DataAccess.Memory.Repositories;
using MG_Project.Abstractions;
using MG_Project.Services;
using MG_Project.ServiceAbstractions;
using Xunit;
using System;

namespace MG_Project.Test
{
    public class InMemoryServicesFixture
    {
        public DataSeeder DataSeeder { get; }

        public IAddressService AddressService { get; }
        public IPersonService PersonService { get; }
        public IEmployeeService EmployeeService { get; }
        public IDepartmentService DepartmentService { get; }
        public IFirmService FirmService { get; }
        public ITasksService TasksService { get; }

        public InMemoryServicesFixture()
        {
            var db = new MemoryDbContext();

            // Repozytoria
            IAddressRepositories addressRepo = new AddressRepositoryMemory(db);
            IPersonRepositories personRepo = new PersonRepositoryMemory(db);
            IEmployeeRepositories employeeRepo = new EmployeeRepositoryMemory(db);
            IDepartmentRepositories departmentRepo = new DepartmentRepositoryMemory(db);
            IFirmRepositories firmRepo = new FirmRepositoryMemory(db);
            ITasksRepositories tasksRepo = new TasksRepositoryMemory(db);

            // UnitOfWork
            IUnitOfWork uow = new UnitOfWorkMemory(db);

            // Serwisy
            AddressService = new AddressService(addressRepo, uow);
            PersonService = new PersonService(addressRepo, personRepo, uow);
            DepartmentService = new DepartmentService(departmentRepo, uow);
            FirmService = new FirmService(firmRepo, uow);
            TasksService = new TasksService(tasksRepo, uow);
            EmployeeService = new EmployeeService(employeeRepo, personRepo, DepartmentService, FirmService, uow);



            // DataSeeder
            DataSeeder = new DataSeeder(AddressService, PersonService, EmployeeService, DepartmentService, FirmService, TasksService);
            DataSeeder.Seed();
        }
    }

    public class DataSeederTests : IClassFixture<InMemoryServicesFixture>
    {
        private readonly InMemoryServicesFixture _fixture;

        public DataSeederTests(InMemoryServicesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Seed_ShouldCreateAllEntities()
        {
            var result = _fixture.DataSeeder.Seed();

            Assert.True(result.Success);
            Assert.NotEqual(Guid.Empty, result.AddrA);
            Assert.NotEqual(Guid.Empty, result.AddrB);
            Assert.NotEqual(Guid.Empty, result.PerA);
            Assert.NotEqual(Guid.Empty, result.PerB);
            Assert.NotEqual(Guid.Empty, result.EmpA);
            Assert.NotEqual(Guid.Empty, result.EmpB);
            Assert.NotEqual(Guid.Empty, result.DeptA);
            Assert.NotEqual(Guid.Empty, result.DeptB);
            Assert.NotEqual(Guid.Empty, result.FirmA);
            Assert.NotEqual(Guid.Empty, result.FirmB);
            Assert.NotEqual(Guid.Empty, result.Task1);
            Assert.NotEqual(Guid.Empty, result.Task2);
        }
    }
}
