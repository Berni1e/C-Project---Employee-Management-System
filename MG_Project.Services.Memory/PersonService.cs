using MG_Project.Abstractions;
using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MG_Project.Services
{
    public class PersonService : IPersonService
    {
        private readonly IAddressRepositories _addresses;
        private readonly IPersonRepositories _persons;
        private readonly IUnitOfWork _uow;

        public PersonService(IAddressRepositories addresses, IPersonRepositories persons, IUnitOfWork uow)
        {
            _addresses = addresses;
            _persons = persons;
            _uow = uow;
        }

        public Guid AddPerson(string firstName, string lastName, string pesel, DateTime dateOfBirth, Address address)
        {
            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                Pesel = pesel,
                DateOfBirth = dateOfBirth,
                Address = address
            };

            _persons.Add(person);
            _uow.SaveChanges();
            return person.PersonId;
        }

        public Person? Get(Guid id)
        {
            return _persons.Get(id);
        }

        public IReadOnlyList<Person> FullAddr()
        {
            return new ReadOnlyCollection<Person>(_persons.Query().ToList());
        }
    }
}
