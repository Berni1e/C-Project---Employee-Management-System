using System;

namespace MG_Project.Desktop.ViewModels.Firms
{
    public class FirmListItemVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string City { get; set; } = ""; // Dodatkowa informacja
        public int EmployeesCount { get; set; } // Liczba pracowników
    }
}