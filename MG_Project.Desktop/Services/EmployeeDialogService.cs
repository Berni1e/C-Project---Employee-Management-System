using MG_Project.Desktop.ViewModels;
using MG_Project.Desktop.Windows;
using System;
using System.Windows;

namespace MG_Project.Desktop.Services
{
    public class EmployeeDialogService : IEmployeeDialogService
    {
        // Fabryki okien (dostarczane przez DI)
        private readonly Func<AddEmployeeWindow> _addFactory;
        private readonly Func<EditEmployeeWindow> _editFactory;

        public EmployeeDialogService(Func<AddEmployeeWindow> addFactory, Func<EditEmployeeWindow> editFactory)
        {
            _addFactory = addFactory;
            _editFactory = editFactory;
        }

        public AddEmployeeViewModel? OpenAddDialog()
        {
            var window = _addFactory();
            if (window.ShowDialog() == true)
            {
                return window.DataContext as AddEmployeeViewModel;
            }
            return null;
        }

        public bool Confirm(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public EditEmployeeViewModel? OpenEditDialog(EditEmployeeViewModel vm)
        {
            // 1. Pobieramy "czyste" okno z fabryki
            var window = _editFactory();

            // 2. Ręcznie przypisujemy ViewModel
            window.DataContext = vm;

            // 3. Subskrybujemy zdarzenie zamknięcia (CloseRequested) TUTAJ
            // Ponieważ okno nie robi tego już w konstruktorze
            void CloseHandler(object? sender, bool result)
            {
                window.DialogResult = result;
                window.Close();
            }
            ;

            vm.CloseRequested += CloseHandler;

            // 4. Wyświetlamy okno
            var dialogResult = window.ShowDialog();

            // 5. Sprzątamy (odsubskrybowanie zdarzenia, dobra praktyka)
            vm.CloseRequested -= CloseHandler;

            if (dialogResult == true)
            {
                return window.DataContext as EditEmployeeViewModel;
            }
            return null;
        }
    }
}
