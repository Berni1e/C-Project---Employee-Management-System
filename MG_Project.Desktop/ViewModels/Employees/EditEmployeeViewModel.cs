using MG_Project.DataModel;
using MG_Project.Desktop.Infrastructure;
using MG_Project.ServiceAbstractions; // WAŻNE: Dodaj ten using dla IFirmService
using System;
using System.Collections.ObjectModel; // WAŻNE: Dla ObservableCollection
using System.Linq;

namespace MG_Project.Desktop.ViewModels
{
    public class EditEmployeeViewModel : BaseViewModel
    {
        public Guid EmployeeId { get; }

        private string _firstName;
        public string FirstName { get => _firstName; set => RaiseAndSetIfChanged(ref _firstName, value); }

        private string _lastName;
        public string LastName { get => _lastName; set => RaiseAndSetIfChanged(ref _lastName, value); }

        private string _position;
        public string Position { get => _position; set => RaiseAndSetIfChanged(ref _position, value); }

        private double _baseSalary;
        public double BaseSalary { get => _baseSalary; set => RaiseAndSetIfChanged(ref _baseSalary, value); }

        // --- NOWOŚĆ: Obsługa Firm ---

        // Lista wszystkich firm dostępnych w bazie (do ComboBoxa)
        public ObservableCollection<Firm> AvailableFirms { get; }

        // Aktualnie wybrana firma
        private Firm? _selectedFirm;
        public Firm? SelectedFirm
        {
            get => _selectedFirm;
            set => RaiseAndSetIfChanged(ref _selectedFirm, value);
        }

        // -----------------------------

        public event EventHandler<bool>? CloseRequested;
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        // ZMODYFIKOWANY KONSTRUKTOR: Dodaliśmy IFirmService
        public EditEmployeeViewModel(Employee employee, IFirmService firmService)
        {
            EmployeeId = employee.EmployeeId;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Position = employee.Position;
            BaseSalary = employee.BaseSalary;

            // 1. Pobranie listy firm z serwisu (z bazy SQL)
            var firmsList = firmService.GetAll().ToList();
            AvailableFirms = new ObservableCollection<Firm>(firmsList);

            // 2. Ustawienie aktualnej firmy (jeśli pracownik jakąś ma)
            if (employee.FirmId.HasValue)
            {
                // Szukamy firmy na liście po ID, żeby ComboBox wiedział co zaznaczyć
                SelectedFirm = AvailableFirms.FirstOrDefault(f => f.FirmId == employee.FirmId);
            }

            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, true));
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(this, false));
        }
    }
}