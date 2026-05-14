using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MG_Project.Test
{
    public class EmployeeServiceTests : IClassFixture<InMemoryServicesFixture>
    {
        private readonly InMemoryServicesFixture _fixture;

        public EmployeeServiceTests(InMemoryServicesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddEmployee_ShouldCreateEmployee()
        {
            var addr = _fixture.AddressService.FullAddr()[0];
            var firmId = _fixture.DataSeeder.Seed().FirmA;
            var deptId = _fixture.DataSeeder.Seed().DeptA;

            var emp = _fixture.EmployeeService.AddEmployee(
                Guid.NewGuid(),
                "Adam",
                "Nowak",
                "98765432101",
                DateTime.Now.AddYears(-25),
                addr,
                "Tester",
                4000,
                500,
                DateTime.Now,
                new System.Collections.Generic.List<MG_Project.DataModel.Tasks>(),
                deptId,
                firmId
            );

            Assert.NotNull(emp);
            Assert.Equal("Adam", emp.FirstName);
            Assert.Equal(firmId, emp.FirmId);
            Assert.Equal(deptId, emp.DepartmentId);
        }

        [Fact]
        public void CalcSalary_ShouldUpdateBaseSalary()
        {
            var employees = _fixture.EmployeeService.CalcSalary();
            Assert.All(employees, e => Assert.True(e.BaseSalary > 0));
        }
    }
}
