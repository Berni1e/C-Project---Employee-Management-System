using MG_Project.Desktop.Infrastructure;
using MG_Project.Desktop.ViewModels;
using System;

namespace MG_Project.Desktop.ViewModels.Firms
{
    public class AddFirmViewModel : BaseViewModel
    {
        public string Name { get; set; } = "";

        // Adres siedziby
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public int HouseNumber { get; set; }

        public event EventHandler<bool>? CloseRequested;
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public AddFirmViewModel()
        {
            SaveCommand = new RelayCommand(() => CloseRequested?.Invoke(this, true));
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(this, false));
        }
    }
}