using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using System;
using System.Collections.Generic;

namespace MG_Project.Services
{
    public class DataSeeder : IDataSeeder
    {
        private readonly IAddressService _addrService;
        private readonly IPersonService _perService;
        private readonly IEmployeeService _empService;
        private readonly IDepartmentService _deptService;
        private readonly IFirmService _firmService;
        private readonly ITasksService _tasksService;

        public DataSeeder(
            IAddressService addrService,
            IPersonService perService,
            IEmployeeService empService,
            IDepartmentService deptService,
            IFirmService firmService,
            ITasksService tasksService)
        {
            _addrService = addrService;
            _perService = perService;
            _empService = empService;
            _deptService = deptService;
            _firmService = firmService;
            _tasksService = tasksService;
        }

        public SeedResult Seed()
        {
            // --- Address seeding ---
            var AddrA = _addrService.AddAddress("Strzelecka", "Kraków", "Polska", "43-234", 32, 65);
            var AddrB = _addrService.AddAddress("Pułkownicka", "Sielanki", "Polska", "53-233", 54, 78);
            var AddrC = _addrService.AddAddress("Pulchna", "Poznań", "Polska", "31-245", 32, null);
            var AddrD = _addrService.AddAddress("Krasowa", "Warszawa", "Polska", "03-214", 54, null);

            var AddrAObj = _addrService.Get(AddrA.AddressId)!;
            var AddrBObj = _addrService.Get(AddrB.AddressId)!;

            // --- Department seeding ---
            var DeptAId = _deptService.AddDep(Guid.NewGuid(), "Development", new List<Employee> { });
            var DeptBId = _deptService.AddDep(Guid.NewGuid(), "Management", new List<Employee> { });

            var DeptAObj = _deptService.Get(DeptAId)!;
            var DeptBObj = _deptService.Get(DeptBId)!;

            // --- Firm seeding ---
            var FirmAId = _firmService.AddFirm(Guid.NewGuid(), "TechCorp", AddrC, new List<Department>());
            var FirmBId = _firmService.AddFirm(Guid.NewGuid(), "BizGroup", AddrD, new List<Department>());

            var FirmAObj = _firmService.Get(FirmAId)!;
            var FirmBObj = _firmService.Get(FirmBId)!;

            // --- Person seeding ---
            var PerA = _perService.AddPerson("Gracjan", "Punktowski", "02342353775", new DateTime(2019, 5, 15), AddrA);
            var PerB = _perService.AddPerson("Robert", "Lee", "05664375445", new DateTime(2014, 6, 5), AddrB);

            // --- Employee seeding ---
            var EmpA = _empService.AddEmployee(
                Guid.NewGuid(),
                "Gracjan",
                "Punktowski",
                "02342353775",
                new DateTime(2019, 5, 15),
                AddrAObj,
                "Developer",
                6000,
                1200,
                DateTime.Now.AddYears(-2),
                new List<Tasks>(),
                departmentId: null,
                firmId: FirmAId
            );

            var EmpB = _empService.AddEmployee(
                Guid.NewGuid(),
                "Robert",
                "Lee",
                "05664375445",
                new DateTime(2014, 6, 5),
                AddrBObj,
                "Manager",
                8000,
                2000,
                DateTime.Now.AddYears(-5),
                new List<Tasks>(),
                departmentId: null,
                firmId: FirmBId
            );

            // --- przypisanie Department i Firm do Employee ---
            EmpA.DepartmentId = DeptAId;
            EmpA.Department = DeptAObj;
            EmpA.FirmId = FirmAId;
            EmpA.Firm = FirmAObj;

            EmpB.DepartmentId = DeptBId;
            EmpB.Department = DeptBObj;
            EmpB.FirmId = FirmBId;
            EmpB.Firm = FirmBObj;

            // --- Task seeding ---
            var Task1Id = _tasksService.AddTask(Guid.NewGuid(),
                "Implement login feature",
                "Create and integrate user login functionality",
                DateTime.Now.AddDays(14),
                false);

            var Task2Id = _tasksService.AddTask(Guid.NewGuid(),
                "Prepare quarterly report",
                "Compile and submit the quarterly financial report",
                DateTime.Now.AddDays(7),
                false);

            // pobranie obiektów Tasks
            var Task1 = _tasksService.get(Task1Id)!;
            var Task2 = _tasksService.get(Task2Id)!;

            // przypisanie tasków do pracowników
            EmpA.TaskList.Add(Task1);
            EmpB.TaskList.Add(Task2);

            return new SeedResult
            {
                Success = true,
                Message = "Seeding completed successfully",

                AddrA = AddrA.AddressId,
                AddrB = AddrB.AddressId,

                PerA = PerA,
                PerB = PerB,

                DeptA = DeptAId,
                DeptB = DeptBId,

                FirmA = FirmAId,
                FirmB = FirmBId,

                Task1 = Task1Id,
                Task2 = Task2Id,

                EmpA = EmpA.EmployeeId,
                EmpB = EmpB.EmployeeId,
            };
        }
    }
}
