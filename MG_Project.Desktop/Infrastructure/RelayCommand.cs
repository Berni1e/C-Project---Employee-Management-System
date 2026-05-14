using System;
using System.Windows.Input;

namespace MG_Project.Desktop.Infrastructure
{

    // Implementacja „komendy pośredniczącej” (RelayCommand).
    // Implementuje interfejs ICommand, który umożliwia w WPF
    // powiązanie akcji UI (np. kliknięcia Buttona)
    // z logiką w ViewModelu, zamiast w code-behind.
    public class RelayCommand : ICommand
    {
        // Delegat zawierający logikę do wykonania
        // (co ma się stać po kliknięciu przycisku).
        private readonly Action _execute;

        // Opcjonalny delegat określający,
        // czy komenda może być w danym momencie wykonana.
        // Jeśli zwraca false, kontrolki powiązane z komendą
        // (np. Button) są automatycznie wyszarzone.
        private readonly Func<bool>? _canExecute;

        // Konstruktor przyjmuje:
        // - delegat wykonania (execute)
        // - delegat warunku wykonania (canExecute)
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // Metoda wywoływana przez WPF automatycznie.
        // WPF pyta: "czy przycisk ma być aktywny?"
        public bool CanExecute(object? parameter)
            => _canExecute?.Invoke() ?? true;
        // jeśli delegat warunku nie został podany - zawsze true

        // Metoda wywoływana przez WPF po kliknięciu przycisku.
        // Wywołuje delegat wykonania z ViewModelu.
        public void Execute(object? parameter)
            => _execute();

        // Zdarzenie, na które nasłuchuje WPF.
        // Po jego wywołaniu WPF ponownie sprawdzi CanExecute()
        // i zaktualizuje stan przycisku (enabled / disabled).
        public event EventHandler? CanExecuteChanged;

        // Metoda pomocnicza do zgłoszenia,
        // że warunki wykonania komendy się zmieniły
        // (np. zmieniło się SelectedItem).
        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
