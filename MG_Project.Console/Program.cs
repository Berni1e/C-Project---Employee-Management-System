using MG_Project.DataAccess.Memory;
using MG_Project.DataAccess.Memory.Repositories;
using MG_Project.Services;
using MG_Project.ServiceAbstractions;
using MG_Project.Abstractions;
using MG_Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

// Alias, aby uniknąć konfliktu nazw z System.Threading.Tasks
using Tasks = MG_Project.DataModel.Tasks;

namespace MG_Project.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. KONFIGURACJA ZALEŻNOŚCI
            var db = new MemoryDbContext();

            // Repozytoria (Używane tylko do konstrukcji serwisów!)
            IAddressRepositories addressRepo = new AddressRepositoryMemory(db);
            IPersonRepositories personRepo = new PersonRepositoryMemory(db);
            IEmployeeRepositories employeeRepo = new EmployeeRepositoryMemory(db);
            IDepartmentRepositories departmentRepo = new DepartmentRepositoryMemory(db);
            IFirmRepositories firmRepo = new FirmRepositoryMemory(db);
            ITasksRepositories tasksRepo = new TasksRepositoryMemory(db);

            // Unit of Work
            IUnitOfWork uow = new UnitOfWorkMemory(db);

            // Serwisy
            var addressService = new AddressService(addressRepo, uow);
            var departmentService = new DepartmentService(departmentRepo, uow);
            var firmService = new FirmService(firmRepo, uow);
            var tasksService = new TasksService(tasksRepo, uow);

            // Serwisy złożone
            var personService = new PersonService(addressRepo, personRepo, uow);
            var employeeService = new EmployeeService(employeeRepo, personRepo, departmentService, firmService, uow);

            // 2. SEEDOWANIE DANYCH
            var seeder = new DataSeeder(addressService, personService, employeeService, departmentService, firmService, tasksService);
            var seedResult = seeder.Seed();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(seedResult.Success ? "✔ Baza danych zainicjalizowana pomyślnie." : "❌ Błąd inicjalizacji bazy.");
            Console.ResetColor();

            // 3. PĘTLA GŁÓWNA PROGRAMU
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("    SYSTEM ZARZĄDZANIA PRACOWNIKAMI");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Pracownicy (Dodawanie, Edycja, Lista)");
                Console.WriteLine("2. Działy (Struktura, Przypisywanie)");
                Console.WriteLine("3. Firmy (Zarządzanie, Adresy siedzib)");
                Console.WriteLine("4. Zadania (Przydzielanie zadań)");
                Console.WriteLine("5. Raporty i Wyszukiwanie");
                Console.WriteLine("0. Wyjście");
                Console.WriteLine("------------------------------------------");
                Console.Write("Wybierz opcję: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        MenuEmployees(employeeService, departmentService, uow);
                        break;
                    case "2":
                        MenuDepartments(departmentService, firmService, uow);
                        break;
                    case "3":
                        MenuFirms(firmService, uow);
                        break;
                    case "4":
                        MenuTasks(employeeService, uow);
                        break;
                    case "5":
                        MenuSearch(employeeService);
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Zamykanie systemu...");
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                        break;
                }
            }
        }

        // ==========================================
        // SEKCJA: PRACOWNICY (Refaktoryzacja: Używa IEmployeeService)
        // ==========================================
        static void MenuEmployees(
            EmployeeService employeeService,
            DepartmentService departmentService,
            IUnitOfWork uow)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- ZARZĄDZANIE PRACOWNIKAMI ---");
                Console.WriteLine("1. Lista wszystkich pracowników (Szczegóły)");
                Console.WriteLine("2. Dodaj nowego pracownika");
                Console.WriteLine("3. Edytuj dane pracownika (Stanowisko/Płaca)");
                Console.WriteLine("4. Usuń pracownika");
                Console.WriteLine("5. Przypisz pracownika do działu");
                Console.WriteLine("0. Powrót");
                Console.Write("Wybór: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("\nLISTA PRACOWNIKÓW:");
                        foreach (var e in employeeService.FullInfo())
                        {
                            Console.WriteLine("--------------------------------");
                            Console.WriteLine(e.FullInfo());
                        }
                        break;

                    case "2":
                        AddEmployeeUI(employeeService);
                        break;

                    case "3":
                        EditEmployeeUI(employeeService, uow);
                        break;

                    case "4":
                        RemoveEmployeeUI(employeeService);
                        break;

                    case "5":
                        AssignEmpToDeptUI(employeeService, departmentService, uow);
                        break;

                    case "0":
                        back = true;
                        break;
                }
            }
        }

        static void AddEmployeeUI(EmployeeService employeeService)
        {
            Console.WriteLine("\n--- NOWY PRACOWNIK ---");
            Console.Write("Imię: "); string fn = Console.ReadLine();
            Console.Write("Nazwisko: "); string ln = Console.ReadLine();
            Console.Write("PESEL: "); string pesel = Console.ReadLine();
            Console.Write("Data urodzenia (rrrr-mm-dd): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime dob);

            Console.WriteLine("\n[Adres Zamieszkania]");
            Console.Write("Ulica: "); string street = Console.ReadLine();
            Console.Write("Miasto: "); string city = Console.ReadLine();
            Console.Write("Kod pocztowy: "); string zip = Console.ReadLine();
            Console.Write("Nr domu: "); int.TryParse(Console.ReadLine(), out int hn);
            Console.Write("Nr lokalu (opcjonalnie): ");
            string aptInput = Console.ReadLine();
            int? an = string.IsNullOrEmpty(aptInput) ? null : int.Parse(aptInput);

            var address = new Address(street, city, "Polska", zip, hn, an);

            Console.WriteLine("\n[Dane Zatrudnienia]");
            Console.Write("Stanowisko: "); string pos = Console.ReadLine();
            Console.Write("Wynagrodzenie zasadnicze: "); double.TryParse(Console.ReadLine(), out double salary);
            Console.Write("Premia: "); double.TryParse(Console.ReadLine(), out double bonus);

            try
            {
                employeeService.AddEmployee(
                    Guid.NewGuid(), fn, ln, pesel, dob, address,
                    pos, salary, bonus, DateTime.Now,
                    new List<Tasks>(), null, null
                );
                Console.WriteLine("✔ Dodano pracownika.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Błąd: {ex.Message}");
            }
        }

        static void EditEmployeeUI(EmployeeService service, IUnitOfWork uow)
        {
            Console.Write("Podaj ID pracownika: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                var emp = service.get(id);
                if (emp != null)
                {
                    Console.WriteLine($"Edycja: {emp.FirstName} {emp.LastName}");
                    Console.Write($"Nowe stanowisko (obecnie: {emp.Position}): ");
                    string p = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(p)) emp.Position = p;

                    Console.Write($"Nowa pensja (obecnie: {emp.BaseSalary}): ");
                    string s = Console.ReadLine();
                    if (double.TryParse(s, out double sal)) emp.BaseSalary = sal;

                    uow.SaveChanges();
                    Console.WriteLine("✔ Zaktualizowano.");
                }
                else Console.WriteLine("❌ Nie znaleziono pracownika.");
            }
        }

        static void RemoveEmployeeUI(EmployeeService service)
        {
            Console.Write("Podaj ID pracownika do usunięcia: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                bool success = service.RemoveEmployee(id);

                if (success) Console.WriteLine("✔ Pracownik usunięty.");
                else Console.WriteLine("❌ Nie znaleziono lub błąd usuwania.");
            }
        }

        static void AssignEmpToDeptUI(EmployeeService empService, DepartmentService deptService, IUnitOfWork uow)
        {
            Console.Write("ID Pracownika: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid eId)) return;

            Console.Write("ID Działu: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid dId)) return;

            var emp = empService.get(eId);
            var dept = deptService.Get(dId);

            if (emp != null && dept != null)
            {
                emp.Department = dept;
                emp.DepartmentId = dept.DepartmentId;
                dept.AddEmp(emp);

                uow.SaveChanges();
                Console.WriteLine($"✔ Przypisano {emp.FirstName} {emp.LastName} do działu {dept.DeptName}.");
            }
            else Console.WriteLine("❌ Błąd danych (nie znaleziono pracownika lub działu).");
        }

        // ==========================================
        // SEKCJA: DZIAŁY (Używa Serwisów)
        // ==========================================
        static void MenuDepartments(DepartmentService deptService, FirmService firmService, IUnitOfWork uow)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- ZARZĄDZANIE DZIAŁAMI ---");
                Console.WriteLine("1. Lista działów");
                Console.WriteLine("2. Dodaj nowy dział");
                Console.WriteLine("3. Przypisz dział do firmy");
                Console.WriteLine("0. Powrót");
                Console.Write("Wybór: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        foreach (var d in deptService.GetAllDepartments())
                        {
                            Console.WriteLine($"ID: {d.DepartmentId} | Nazwa: {d.DeptName} | Pracowników: {d.EmpList.Count}");
                        }
                        break;
                    case "2":
                        Console.Write("Nazwa działu: ");
                        string name = Console.ReadLine();
                        deptService.AddDep(Guid.NewGuid(), name, new List<Employee>());
                        Console.WriteLine("✔ Dodano dział.");
                        break;
                    case "3":
                        Console.Write("ID Działu: ");
                        Guid.TryParse(Console.ReadLine(), out Guid did);
                        Console.Write("ID Firmy: ");
                        Guid.TryParse(Console.ReadLine(), out Guid fid);

                        var dep = deptService.Get(did);
                        var firm = firmService.Get(fid);

                        if (dep != null && firm != null)
                        {
                            firm.AddDep(dep);
                            uow.SaveChanges();
                            Console.WriteLine($"✔ Dział {dep.DeptName} przypisany do firmy {firm.Name}.");
                        }
                        else Console.WriteLine("❌ Nie znaleziono obiektu.");
                        break;
                    case "0":
                        back = true;
                        break;
                }
            }
        }

        // ==========================================
        // SEKCJA: FIRMY (Refaktoryzacja: Używa IFirmService)
        // ==========================================
        static void MenuFirms(FirmService firmService, IUnitOfWork uow)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- ZARZĄDZANIE FIRMAMI ---");
                Console.WriteLine("1. Lista firm");
                Console.WriteLine("2. Szczegóły firmy (Pracownicy, Działy, Adres)");
                Console.WriteLine("3. Dodaj firmę");
                Console.WriteLine("0. Powrót");
                Console.Write("Wybór: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        foreach (var f in firmService.GetAll())
                            Console.WriteLine($"ID: {f.FirmId} | Nazwa: {f.Name}");
                        break;

                    case "2":
                        Console.Write("Podaj ID firmy: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid fId))
                        {
                            // Zmiana: repo.Get() -> service.Get()
                            var firm = firmService.Get(fId);
                            if (firm != null)
                            {
                                Console.WriteLine($"\nFirma: {firm.Name}");
                                if (firm.Address != null)
                                {
                                    Console.WriteLine($"Siedziba: {firm.Address.FullAddr()}");
                                }
                                firm.DispAllDep();
                                firm.DispAllEmp();
                            }
                            else Console.WriteLine("Nie znaleziono firmy.");
                        }
                        break;

                    case "3":
                        Console.WriteLine("\n[Dodawanie nowej firmy]");
                        Console.Write("Nazwa firmy: ");
                        string name = Console.ReadLine();

                        Console.WriteLine("--- Adres Siedziby ---");
                        Console.Write("Ulica: "); string st = Console.ReadLine();
                        Console.Write("Miasto: "); string ct = Console.ReadLine();
                        Console.Write("Kod pocztowy: "); string z = Console.ReadLine();
                        Console.Write("Nr budynku: "); int.TryParse(Console.ReadLine(), out int h);

                        var firmAddr = new Address(st, ct, "Polska", z, h, null);
                        firmService.AddFirm(Guid.NewGuid(), name, firmAddr, new List<Department>());

                        Console.WriteLine("✔ Firma dodana pomyślnie.");
                        break;

                    case "0":
                        back = true;
                        break;
                }
            }
        }

        // ==========================================
        // SEKCJA: ZADANIA (Refaktoryzacja: Używa IEmployeeService)
        // ==========================================
        static void MenuTasks(EmployeeService empService, IUnitOfWork uow)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- ZADANIA I KOMPETENCJE ---");
                Console.WriteLine("1. Przydziel zadanie pracownikowi");
                Console.WriteLine("2. Zobacz zadania pracownika");
                Console.WriteLine("3. Oznacz zadanie jako wykonane");
                Console.WriteLine("0. Powrót");
                Console.Write("Wybór: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Podaj ID Pracownika: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid eid))
                        {
                            var emp = empService.get(eid);
                            if (emp != null)
                            {
                                Console.Write("Tytuł zadania: "); string t = Console.ReadLine();
                                Console.Write("Opis: "); string d = Console.ReadLine();
                                Console.Write("Dni na wykonanie: ");
                                int.TryParse(Console.ReadLine(), out int days);

                                var newTask = new Tasks(t, d, DateTime.Now.AddDays(days));
                                emp.AddTask(newTask);
                                uow.SaveChanges();
                                Console.WriteLine("✔ Zadanie przydzielone.");
                            }
                            else Console.WriteLine("Nie znaleziono pracownika.");
                        }
                        break;

                    case "2":
                        Console.Write("Podaj ID Pracownika: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid viewId))
                        {
                            var emp = empService.get(viewId);
                            if (emp != null) emp.ShowAllTasks();
                            else Console.WriteLine("Nie znaleziono pracownika.");
                        }
                        break;

                    case "3":
                        MarkTaskDoneUI(empService, uow);
                        break;

                    case "0":
                        back = true;
                        break;
                }
            }
        }

        static void MarkTaskDoneUI(EmployeeService empService, IUnitOfWork uow)
        {
            Console.WriteLine("\n--- OZNACZANIE ZADANIA JAKO WYKONANE ---");
            Console.Write("Podaj ID Pracownika: ");

            if (Guid.TryParse(Console.ReadLine(), out Guid empId))
            {
                var emp = empService.get(empId);
                if (emp == null)
                {
                    Console.WriteLine("❌ Nie znaleziono pracownika o podanym ID.");
                    return;
                }

                Console.WriteLine($"\nZadania pracownika {emp.FirstName} {emp.LastName}:");
                if (emp.TaskList.Count == 0)
                {
                    Console.WriteLine("Brak przypisanych zadań.");
                    return;
                }

                foreach (var task in emp.TaskList)
                {
                    string status = task.IsDone ? "[ZROBIONE]" : "[DO ZROBIENIA]";
                    Console.WriteLine($"ID: {task.TasksId} | {status} | {task.Title}");
                }

                Console.WriteLine("\nPodaj ID zadania do oznaczenia jako wykonane:");
                if (Guid.TryParse(Console.ReadLine(), out Guid taskId))
                {
                    var taskToUpdate = emp.TaskList.FirstOrDefault(t => t.TasksId == taskId);
                    if (taskToUpdate != null)
                    {
                        if (taskToUpdate.IsDone)
                        {
                            Console.WriteLine("ℹ To zadanie jest już zakończone.");
                        }
                        else
                        {
                            taskToUpdate.MarkAsDone();
                            uow.SaveChanges();
                            Console.WriteLine("✔ Zadanie oznaczone jako wykonane!");
                        }
                    }
                    else Console.WriteLine("❌ Nie znaleziono zadania.");
                }
                else Console.WriteLine("❌ Nieprawidłowy format ID zadania.");
            }
            else Console.WriteLine("❌ Nieprawidłowy format ID pracownika.");
        }

        // ==========================================
        // SEKCJA: WYSZUKIWANIE (Refaktoryzacja: Używa IEmployeeService)
        // ==========================================
        static void MenuSearch(EmployeeService empService)
        {
            Console.WriteLine("\n--- WYSZUKIWANIE ---");
            Console.WriteLine("1. Szukaj po nazwisku");
            Console.WriteLine("2. Filtruj po dziale");
            Console.WriteLine("3. Filtruj po stanowisku");
            Console.WriteLine("0. Powrót");
            Console.Write("Wybór: ");

            var input = Console.ReadLine();
            if (input == "0") return;

            var employees = empService.FullInfo();
            string query;

            switch (input)
            {
                case "1":
                    Console.Write("Podaj nazwisko (lub fragment): ");
                    query = Console.ReadLine() ?? "";
                    var byName = employees
                        .Where(e => e.LastName.Contains(query, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    PrintResults(byName);
                    break;

                case "2":
                    Console.Write("Podaj nazwę działu: ");
                    query = Console.ReadLine() ?? "";
                    var byDept = employees
                        .Where(e => e.Department != null &&
                                    e.Department.DeptName.Contains(query, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    PrintResults(byDept);
                    break;

                case "3":
                    Console.Write("Podaj stanowisko: ");
                    query = Console.ReadLine() ?? "";
                    var byPos = employees
                        .Where(e => e.Position.Contains(query, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    PrintResults(byPos);
                    break;
            }
        }

        static void PrintResults(IEnumerable<Employee> results)
        {
            var list = results.ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("Brak wyników.");
            }
            else
            {
                Console.WriteLine($"\nZnaleziono {list.Count} pracowników:");
                foreach (var e in list)
                {
                    string deptName = e.Department != null ? e.Department.DeptName : "Brak działu";
                    Console.WriteLine($"- {e.FirstName} {e.LastName}, Stanowisko: {e.Position}, Dział: {deptName}");
                }
            }
        }
    }
}