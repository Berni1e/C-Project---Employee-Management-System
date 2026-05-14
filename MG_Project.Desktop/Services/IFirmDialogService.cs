using MG_Project.Desktop.ViewModels.Firms;

namespace MG_Project.Desktop.Services
{
    public interface IFirmDialogService
    {
        AddFirmViewModel? OpenAddDialog();
        EditFirmViewModel? OpenEditDialog(EditFirmViewModel vm);
        bool Confirm(string message, string title);
    }
}