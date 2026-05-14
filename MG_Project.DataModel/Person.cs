using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataModel
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }

        public Person()
        {
            PersonId = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            Pesel = string.Empty;
            DateOfBirth = DateTime.MinValue;
            Address = new Address();
        }
        public Person(string firstName, string lastName, string pesel, DateTime dateOfBirth, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            Pesel = pesel;
            DateOfBirth = dateOfBirth;
            Address = address;
        }

        public virtual string FullInfo()
        {
            return $"First name and last name: {FirstName} {LastName}\n" +
                   $"PESEL: {Pesel}\n" +
                   $"Address: {Address.FullAddr()}";
        }
    }
}

