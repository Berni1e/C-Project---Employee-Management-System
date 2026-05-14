using MG_Project.DataModel;
using MG_Project.Desktop.Infrastructure;
using MG_Project.Desktop.Services;
using MG_Project.Desktop.ViewModels;
using MG_Project.Desktop.ViewModels.Firms;
using MG_Project.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MG_Project.Desktop.ViewModels
{
    public class FirmsViewModel : BaseViewModel
    {
        private readonly IFirmService _firmService;
        private readonly IFirmDialogService _dialogService;

        // Kolekcja firm do listy głównej
        public ObservableCollection<FirmListItemVm> Firms { get; } = new();

        // Kolekcja pracowników wybranej firmy (szczegóły)
        public ObservableCollection<Employee> FirmEmployees { get; } = new();

        public FirmsViewModel(IFirmService firmService, IFirmDialogService dialogService)
        {
            _firmService = firmService;
            _dialogService = dialogService;

            LoadCommand = new RelayCommand(LoadData);
            AddCommand = new RelayCommand(AddFirm);

            // Komendy aktywne tylko gdy wybrano firmę
            EditCommand = new RelayCommand(EditFirm, () => SelectedFirm != null);
            DeleteCommand = new RelayCommand(DeleteFirm, () => SelectedFirm != null);

            LoadData();
        }

        // --- KOMENDY ---
        public RelayCommand LoadCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        // --- WŁAŚCIWOŚCI ---
        private FirmListItemVm? _selectedFirm;
        public FirmListItemVm? SelectedFirm
        {
            get => _selectedFirm;
            set
            {
                if (RaiseAndSetIfChanged(ref _selectedFirm, value))
                    {
                    // Odświeżamy dostępność przycisków
                    EditCommand.RaiseCanExecuteChanged();
                    DeleteCommand.RaiseCanExecuteChanged();

                    // Ładujemy pracowników dla nowo wybranej firmy
                    LoadEmployeesForFirm();
                }
            }
        }

        // --- METODY ---

        private void LoadData()
        {
            Firms.Clear();

            // 1. Pobieramy wszystkie firmy z serwisu
            var firms = _firmService.GetAll();

            foreach (var f in firms)
            {
                // Obliczamy liczbę pracowników (suma z wszystkich działów)
                // Zabezpieczenie ? i ?? chroni przed nullami w DepList lub EmpList
                int empCount = f.DepList?.Sum(d => d.EmpList?.Count ?? 0) ?? 0;

                Firms.Add(new FirmListItemVm
                {
                    Id = f.FirmId,
                    Name = f.Name,
                    City = f.Address?.City ?? "-",
                    EmployeesCount = empCount
                });
            }
        }

        private void LoadEmployeesForFirm()
        {
            FirmEmployees.Clear();
            if (SelectedFirm == null) return;

            // Pobieramy pełny obiekt firmy (wraz z działami i pracownikami)
            var firm = _firmService.Get(SelectedFirm.Id);

            if (firm != null && firm.DepList != null)
            {
                // Iterujemy po działach, żeby wyciągnąć pracowników
                foreach (var dept in firm.DepList)
                {
                    if (dept.EmpList != null)
                    {
                        foreach (var emp in dept.EmpList)
                        {
                            FirmEmployees.Add(emp);
                        }
                    }
                }
            }
        }

        private void AddFirm()
        {
            var vm = _dialogService.OpenAddDialog();
            if (vm != null)
            {
                var addr = new Address(vm.Street, vm.City, "Polska", vm.ZipCode, vm.HouseNumber, null);

                // Dodajemy nową firmę (z pustą listą działów na start)
                _firmService.AddFirm(Guid.NewGuid(), vm.Name, addr, new List<Department>());

                LoadData();
            }
        }

        private void EditFirm()
        {
            if (SelectedFirm == null) return;

            var firm = _firmService.Get(SelectedFirm.Id);
            if (firm == null) return;

            var vm = new EditFirmViewModel(firm);
            var result = _dialogService.OpenEditDialog(vm);

            if (result != null)
            {
                // Aktualizujemy obiekt w pamięci (repozytorium Memory trzyma referencje)
                firm.Name = result.Name;
                if (firm.Address != null)
                {
                    firm.Address.City = result.City;
                }

                LoadData();
            }
        }

        private void DeleteFirm()
        {
            if (SelectedFirm == null) return;

            var confirmed = _dialogService.Confirm(
                $"Czy na pewno chcesz usunąć firmę \"{SelectedFirm.Name}\"?\nUsunięcie firmy usunie również jej działy i przypisania pracowników.",
                "Potwierdzenie usunięcia");

            if (confirmed)
            {
                // Wywołujemy nową metodę Remove z serwisu
                bool success = _firmService.Remove(SelectedFirm.Id);

                if (success)
                {
                    LoadData(); // Odświeżamy listę, firma zniknie
                    SelectedFirm = null; // Czyścimy zaznaczenie
                    FirmEmployees.Clear(); // Czyścimy listę pracowników na dole
                }
            }
        }
    }
}