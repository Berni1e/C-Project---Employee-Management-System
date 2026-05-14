using MG_Project.DataModel;
using MG_Project.Desktop.Infrastructure;
using MG_Project.Desktop.ViewModels;
using System;

namespace MG_Project.Desktop.ViewModels.Firms
{
    public class EditFirmViewModel : BaseViewModel
    {
        public Guid Id { get; }

        private string _name;
        public string Name { get => _name; set => RaiseAndSetIfChanged(ref _name, value); }

        // Adres (można edytować)
        private string _city;
        public string City { get => _city; set => RaiseAndSetIfChanged(ref _city, value); }

        public event EventHandler<bool>? CloseRequested;
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public EditFirmViewModel(Firm firm)
        {
            Id = firm.FirmId;
            Name = firm.Name;
            City = firm.Address?.City ?? ""; // Zabezpieczenie przed null

            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, true));
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(this, false));
        }
    }
}