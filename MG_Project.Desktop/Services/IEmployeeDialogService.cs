using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.Desktop.ViewModels;

namespace MG_Project.Desktop.Services
{
    public interface IEmployeeDialogService
    {
        AddEmployeeViewModel? OpenAddDialog();
        EditEmployeeViewModel? OpenEditDialog(EditEmployeeViewModel vm);
        bool Confirm(string message, string title);
    }
}