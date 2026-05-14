using MG_Project.Abstractions;
using MG_Project.DataAccess.Database; // Tu są Twoje nowe repozytoria SQL
using MG_Project.DataAccess.Database.Repositories;
using MG_Project.DataAccess.Memory; // Zostawiamy dla reszty (Adresy, Osoby - dopóki nie zrobisz migracji dla nich)
using MG_Project.DataAccess.Memory.Repositories;
using MG_Project.Desktop.Services;
using MG_Project.Desktop.ViewModels;
using MG_Project.Desktop.ViewModels.Firms;
using MG_Project.Desktop.Windows;
using MG_Project.ServiceAbstractions;
using MG_Project.Services;
using Microsoft.EntityFrameworkCore; // WAŻNE: Potrzebne do UseSqlServer
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace MG_Project.Desktop
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // =========================================================
            // 1. Rejestracja BAZY DANYCH (Mamy teraz dwie!)
            // =========================================================

            // A) Baza SQL (Dla Pracowników i Firm)
            services.AddDbContext<MGProjectDbContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MG_Project_Db;Trusted_Connection=True;MultipleActiveResultSets=true");
            }, ServiceLifetime.Singleton);

            // B) Baza Pamięci RAM (Dla Address, Person, Tasks etc. - dopóki ich nie przepiszesz)
            // --- DODAJ TĘ LINIĘ ---
            services.AddSingleton<MemoryDbContext>();
            // ----------------------

            // Rejestracja UnitOfWork (Tu jest mały konflikt, ale ustawmy SQL jako główny)
            services.AddSingleton<IUnitOfWork, UnitOfWorkDatabase>();


            // =========================================================
            // 2. Rejestracja Repozytoriów
            // =========================================================

            // SQL
            services.AddSingleton<IEmployeeRepositories, EmployeeRepositoryDatabase>();
            services.AddSingleton<IFirmRepositories, FirmRepositoryDatabase>();

            // MEMORY (One potrzebują MemoryDbContext, który przywróciliśmy wyżej)
            services.AddSingleton<IAddressRepositories, AddressRepositoryMemory>();
            services.AddSingleton<IPersonRepositories, PersonRepositoryMemory>();
            services.AddSingleton<IDepartmentRepositories, DepartmentRepositoryMemory>();
            services.AddSingleton<ITasksRepositories, TasksRepositoryMemory>();

            // =========================================================
            // 3. Rejestracja Serwisów Danych (Bez zmian)
            // =========================================================
            services.AddSingleton<IAddressService, AddressService>();
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IDepartmentService, DepartmentService>();
            services.AddSingleton<IFirmService, FirmService>();
            services.AddSingleton<ITasksService, TasksService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();

            // =========================================================
            // 4. Rejestracja Serwisów Dialogowych
            // =========================================================
            services.AddSingleton<IEmployeeDialogService, EmployeeDialogService>();
            services.AddSingleton<IFirmDialogService, FirmDialogService>();

            // =========================================================
            // 5. Rejestracja ViewModeli
            // =========================================================
            services.AddTransient<EmployeesViewModel>();
            services.AddTransient<FirmsViewModel>();
            services.AddTransient<AddEmployeeViewModel>();
            services.AddTransient<AddFirmViewModel>();

            // =========================================================
            // 6. Rejestracja Okien XAML i Fabryk
            // =========================================================
            services.AddTransient<AddEmployeeWindow>();
            services.AddTransient<EditEmployeeWindow>();
            services.AddSingleton<Func<AddEmployeeWindow>>(s => () => s.GetRequiredService<AddEmployeeWindow>());
            services.AddSingleton<Func<EditEmployeeWindow>>(s => () => s.GetRequiredService<EditEmployeeWindow>());

            services.AddTransient<AddFirmWindow>();
            services.AddTransient<EditFirmWindow>();
            services.AddSingleton<Func<AddFirmWindow>>(s => () => s.GetRequiredService<AddFirmWindow>());
            services.AddSingleton<Func<EditFirmWindow>>(s => () => s.GetRequiredService<EditFirmWindow>());

            // =========================================================
            // 7. Główne Okno
            // =========================================================
            services.AddSingleton<MainWindow>();

            // Budowa kontenera
            Services = services.BuildServiceProvider();

            // =========================================================
            // 8. URUCHOMIENIE SEEDERA SQL (Nowa logika)
            // =========================================================
            // Tworzymy scope, pobieramy bazę i uruchamiamy DataSeederSQL
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MGProjectDbContext>();

                // Upewniamy się, że baza istnieje (opcjonalne, jeśli robisz migracje)
                dbContext.Database.EnsureCreated();

                // Uruchamiamy Twój nowy Seeder SQL
                DataSeederSQL.Seed(dbContext);
            }

            // Uruchomienie aplikacji
            var main = Services.GetRequiredService<MainWindow>();
            main.Show();
        }
    }
}