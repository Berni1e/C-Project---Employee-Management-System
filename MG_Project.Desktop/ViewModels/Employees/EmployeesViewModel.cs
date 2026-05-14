using MG_Project.DataModel;
using MG_Project.Desktop.Infrastructure;
using MG_Project.Desktop.Services;
using MG_Project.ServiceAbstractions;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MG_Project.Desktop.ViewModels;

namespace MG_Project.Desktop.ViewModels
{
    public class EmployeesViewModel : BaseViewModel
    {
        // Pola prywatne (serwisy i bufor danych)
        private readonly IEmployeeService _employeeService;
        private readonly IFirmService _firmService;
        private readonly IEmployeeDialogService _dialogService;
        private List<Employee> _allEmployees; // Lista pomocnicza do filtrowania w pamięci

        // Kolekcja widoczna w widoku (DataGrid/ListBox)
        public ObservableCollection<Employee> Employees { get; } = new();

        // -----------------------------------------------------------
        // KONSTRUKTOR
        // -----------------------------------------------------------
        public EmployeesViewModel(
        IEmployeeService employeeService, IFirmService firmService, IEmployeeDialogService dialogService)
        {
            _employeeService = employeeService;
            _firmService = firmService;
            _dialogService = dialogService;

            // Inicjalizacja komend
            LoadDataCommand = new RelayCommand(LoadData);
            AddCommand = new RelayCommand(AddEmployee);

            // Komenda Edytuj aktywna tylko gdy wybrano pracownika (SelectedEmployee != null)
            EditCommand = new RelayCommand(EditEmployee, () => SelectedEmployee != null);

            DeleteCommand = new RelayCommand(DeleteEmployee, () => SelectedEmployee != null);

            // Pobranie danych przy starcie
            LoadData();
        }

        // -----------------------------------------------------------
        // WŁAŚCIWOŚCI (Properties)
        // -----------------------------------------------------------

        // Aktualnie wybrany pracownik na liście
        private Employee? _selectedEmployee;
        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (RaiseAndSetIfChanged(ref _selectedEmployee, value))
                {
                    // Gdy zmienia się zaznaczenie, odświeżamy stan przycisku "Edytuj"
                    EditCommand.RaiseCanExecuteChanged();

                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Tekst wpisany w wyszukiwarkę
        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (RaiseAndSetIfChanged(ref _searchText, value))
                {
                    // Przy każdej zmianie tekstu filtrujemy listę
                    FilterData();
                }
            }
        }

        // -----------------------------------------------------------
        // KOMENDY (Commands)
        // -----------------------------------------------------------
        public RelayCommand LoadDataCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        // -----------------------------------------------------------
        // METODY PRYWATNE (Logika)
        // -----------------------------------------------------------

        private void LoadData()
        {
            // 1. Pobierz dane z serwisu
            var data = _employeeService.FullInfo();

            // 2. Zapisz je do listy pomocniczej (bufora)
            _allEmployees = data.ToList();

            // 3. Wywołaj filtrowanie, aby przepisać dane do kolekcji Employees
            FilterData();
        }

        private void FilterData()
        {
            // Czyścimy widoczną kolekcję
            Employees.Clear();

            // Jeśli dane nie zostały jeszcze pobrane, przerywamy
            if (_allEmployees == null) return;

            IEnumerable<Employee> query = _allEmployees;

            // Filtrowanie po tekście (Imię, Nazwisko, Stanowisko)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lowerText = SearchText.ToLower();
                query = query.Where(e =>
                    e.FirstName != null && e.FirstName.ToLower().Contains(lowerText) ||
                    e.LastName != null && e.LastName.ToLower().Contains(lowerText) ||
                    e.Position != null && e.Position.ToLower().Contains(lowerText)
                );
            }

            // Przepisujemy wyniki do ObservableCollection
            foreach (var emp in query)
            {
                Employees.Add(emp);
            }
        }

        // Logika dodawania pracownika
        private void AddEmployee()
        {
            var vm = _dialogService.OpenAddDialog();

            if (vm != null)
            {
                var newAddress = new Address(vm.Street, vm.City, "Polska", vm.ZipCode, vm.HouseNumber, null);

                Guid? selectedFirmId = vm.SelectedFirm?.FirmId;

                _employeeService.AddEmployee(
                    Guid.NewGuid(),
                    vm.FirstName,
                    vm.LastName,
                    vm.Pesel,
                    DateTime.Now,
                    newAddress,
                    vm.Position,
                    vm.BaseSalary,
                    0,
                    DateTime.Now,
                    null,
                    selectedFirmId
                );

                LoadData();
            }
        }

        //Usuwanie pracownika
        private void DeleteEmployee()
        {
            if (SelectedEmployee == null) return;

            // 1. Pytamy o potwierdzenie
            bool confirmed = _dialogService.Confirm(
                $"Czy na pewno chcesz usunąć pracownika {SelectedEmployee.FirstName} {SelectedEmployee.LastName}?",
                "Usuwanie pracownika");

            if (confirmed)
            {
                // 2. Wywołujemy Twój serwis
                bool success = _employeeService.RemoveEmployee(SelectedEmployee.EmployeeId);

                if (success)
                {
                    // 3. Odświeżamy listę
                    LoadData();
                    SelectedEmployee = null; // Czyścimy zaznaczenie
                }
            }
        }

        // Logika edycji pracownika
        private void EditEmployee()
        {
            if (SelectedEmployee == null) return;

            // 1. Stwórz VM edycji na bazie zaznaczonego pracownika
            var editVm = new EditEmployeeViewModel(SelectedEmployee, _firmService);

            // 2. Otwórz okno dialogowe
            var result = _dialogService.OpenEditDialog(editVm);

            if (result != null)
            {
                // 3. Zaktualizuj dane w pamięci (MemoryDbContext trzyma referencje)
                // W prawdziwej bazie SQL tutaj wywołałbyś _employeeService.Update(...)
                SelectedEmployee.FirstName = result.FirstName;
                SelectedEmployee.LastName = result.LastName;
                SelectedEmployee.Position = result.Position;
                SelectedEmployee.BaseSalary = result.BaseSalary;

                // 4. Wymuś odświeżenie listy i przywróć zaznaczenie
                LoadData();
                SelectedEmployee = Employees.FirstOrDefault(e => e.EmployeeId == result.EmployeeId);
            }
        }
    }
}