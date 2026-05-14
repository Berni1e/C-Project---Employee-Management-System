using MG_Project.DataModel;
using MG_Project.Desktop.Infrastructure;
using MG_Project.Desktop.ViewModels;
using MG_Project.ServiceAbstractions;
using System;
using System.Collections.ObjectModel;

namespace MG_Project.Desktop.ViewModels
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Pesel { get; set; } = "";
        public string Position { get; set; } = "";
        public double BaseSalary { get; set; }
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public int HouseNumber { get; set; }

        public ObservableCollection<Firm> AvailableFirms { get; } = new();

        private Firm? _selectedFirm;
        public Firm? SelectedFirm
        {
            get => _selectedFirm;
            set => RaiseAndSetIfChanged(ref _selectedFirm, value);
        }

        public event EventHandler<bool>? CloseRequested;
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public AddEmployeeViewModel(IFirmService firmService)
        {
            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, true));
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(this, false));

            var firms = firmService.GetAll();
            if (firms != null)
            {
                foreach (var f in firms) AvailableFirms.Add(f);
            }
        }
    }
}