using System.Windows;

namespace MG_Project.Desktop.Windows
{
    public partial class EditEmployeeWindow : Window
    {
        // Konstruktor przyjmuje ViewModel przygotowany przez DialogService
        public EditEmployeeWindow()
        {
            InitializeComponent();
        }

        private void OnCloseRequested(object? sender, bool dialogResult)
        {
            // Ustawiamy wynik okna (true = Zapisz, false = Anuluj)
            DialogResult = dialogResult;
            Close();
        }
    }
}