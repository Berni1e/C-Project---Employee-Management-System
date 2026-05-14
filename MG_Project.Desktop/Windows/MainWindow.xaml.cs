using MG_Project.Desktop.ViewModels;
using System.Windows;

namespace MG_Project.Desktop
{
    public partial class MainWindow : Window
    {
        // Konstruktor przyjmuje oba ViewModele dzięki Dependency Injection
        public MainWindow(EmployeesViewModel employeesVm, FirmsViewModel firmsVm)
        {
            InitializeComponent();

            // Przypisujemy konkretne ViewModele do konkretnych zakładek (Gridów wewnątrz zakładek)
            // Nazwy "EmployeesView" i "FirmsView" są zdefiniowane w XAML poniżej
            if (EmployeesView != null)
                EmployeesView.DataContext = employeesVm;

            if (FirmsView != null)
                FirmsView.DataContext = firmsVm;
        }
    }
}