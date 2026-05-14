using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MG_Project.Test
{
    public class DepartmentServiceTests : IClassFixture<InMemoryServicesFixture>
    {
        private readonly InMemoryServicesFixture _fixture;
        private readonly IDepartmentService _deptService;

        public DepartmentServiceTests(InMemoryServicesFixture fixture)
        {
            _fixture = fixture;
            _deptService = fixture.DepartmentService;
        }

        [Fact]
        public void AddDep_ShouldCreateDepartment()
        {
            var deptName = "HR";
            var empList = new System.Collections.Generic.List<Employee>();

            var deptId = _deptService.AddDep(System.Guid.NewGuid(), deptName, empList);
            var dept = _deptService.Get(deptId);

            Assert.NotNull(dept);
            Assert.Equal(deptName, dept.DeptName);
            Assert.Empty(dept.EmpList);
        }

        [Fact]
        public void AddEmp_ShouldAddEmployeeToDepartment()
        {
            var dept = _fixture.DepartmentService.GetAllDepartments().First();
            var emp = _fixture.EmployeeService.FullInfo().First();
            var empList = _deptService.AddEmp(dept.DepartmentId, emp);

            Assert.Contains(emp, empList);
            Assert.Equal(dept.DepartmentId, _fixture.DepartmentService.Get(dept.DepartmentId)!.DepartmentId);
        }

        [Fact]
        public void DelEmp_ShouldRemoveEmployeeFromDepartment()
        {
            var dept = _fixture.DepartmentService.GetAllDepartments().First();

            var emp = new Employee
            {
                FirstName = "Test",
                LastName = "User",
                Pesel = "99999999999",
                Address = new Address()
            };

            _deptService.AddEmp(dept.DepartmentId, emp);

            // Usuwamy pracownika
            var empListAfterDelete = _deptService.DelEmp(dept.DepartmentId, emp);

            // Sprawdzenie, czy Dep. zawiera USUNIĘTEGO pracownika
            Assert.DoesNotContain(emp, empListAfterDelete);
        }



        [Fact]
        public void GetAllDepartments_ShouldReturnAllSeededDepartments()
        {
            var allDeps = _deptService.GetAllDepartments();

            Assert.NotEmpty(allDeps);
            Assert.Contains(allDeps, d => d.DeptName == "Development");
            Assert.Contains(allDeps, d => d.DeptName == "Management");
        }
    }
}
