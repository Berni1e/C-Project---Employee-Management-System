using System.Windows;
using MG_Project.Desktop.ViewModels.Firms;

namespace MG_Project.Desktop.Windows
{
    public partial class AddFirmWindow : Window
    {
        // Konstruktor MUSI nazywać się tak jak klasa okna i przyjmować VM firmy
        public AddFirmWindow(AddFirmViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            vm.CloseRequested += (sender, result) =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}