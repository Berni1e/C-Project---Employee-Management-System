using System.Windows;
using MG_Project.Desktop.ViewModels;

namespace MG_Project.Desktop.Windows
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow(AddEmployeeViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            vm.CloseRequested += (s, result) =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}