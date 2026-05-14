using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.DataModel;

namespace MG_Project.ServiceAbstractions
{
    public interface IPersonService
    {
        Guid AddPerson(string firstName, string lastName, string pesel, DateTime dateOfBirth, Address address);
        Person? Get(Guid id);
        IReadOnlyList<Person> FullAddr();
    }
}
