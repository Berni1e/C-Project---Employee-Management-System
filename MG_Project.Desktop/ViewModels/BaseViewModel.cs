using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MG_Project.Desktop.ViewModels
{
    // Bazowa klasa dla wszystkich ViewModeli w aplikacji.
    // Implementuje INotifyPropertyChanged, czyli mechanizm
    // informowania WPF: "dane się zmieniły – odśwież UI".
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        // Zdarzenie, na które nasłuchuje silnik WPF.
        // Gdy je wywołamy, WPF wie, że jakaś właściwość się zmieniła
        // i musi ponownie zaktualizować powiązany widok (binding).
        public event PropertyChangedEventHandler? PropertyChanged;

        // Metoda pomocnicza do zgłaszania zmiany właściwości.
        // [CallerMemberName] powoduje, że NIE musimy podawać
        // nazwy właściwości jako string – kompilator zrobi to za nas.
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Uniwersalna metoda do ustawiania wartości pola + zgłoszenia zmiany.
        // Dzięki niej:
        // - nie powtarzamy tego samego kodu w każdym setterze
        // - unikamy niepotrzebnych odświeżeń UI
        protected bool RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }
}
