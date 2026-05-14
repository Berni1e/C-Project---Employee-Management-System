using System;
using System.Windows;
using MG_Project.Desktop.ViewModels.Firms;
using MG_Project.Desktop.Windows; // Tu będą okna XAML

namespace MG_Project.Desktop.Services
{
    public class FirmDialogService : IFirmDialogService
    {
        private readonly Func<AddFirmWindow> _addFactory;
        private readonly Func<EditFirmWindow> _editFactory;

        public FirmDialogService(Func<AddFirmWindow> addFactory, Func<EditFirmWindow> editFactory)
        {
            _addFactory = addFactory;
            _editFactory = editFactory;
        }

        public AddFirmViewModel? OpenAddDialog()
        {
            var window = _addFactory();
            if (window.ShowDialog() == true)
                return window.DataContext as AddFirmViewModel;
            return null;
        }

        public EditFirmViewModel? OpenEditDialog(EditFirmViewModel vm)
        {
            var window = _editFactory();
            window.DataContext = vm;

            void CloseHandler(object? s, bool r) { window.DialogResult = r; window.Close(); }
            vm.CloseRequested += CloseHandler;

            var result = window.ShowDialog();
            vm.CloseRequested -= CloseHandler;

            if (result == true) return window.DataContext as EditFirmViewModel;
            return null;
        }

        public bool Confirm(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}