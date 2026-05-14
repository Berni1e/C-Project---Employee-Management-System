using MG_Project.Abstractions;
using MG_Project.DataModel;
using MG_Project.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MG_Project.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepositories _addresses;
        private readonly IUnitOfWork _uow;

        public AddressService(IAddressRepositories addresses, IUnitOfWork uow)
        {
            _addresses = addresses;
            _uow = uow;
        }

        public Address AddAddress(string street, string city, string country, string zipcode, int houseNumber, int? apartmentNumber)
        {
            var address = new Address
            {
                Street = street,
                City = city,
                Country = country,
                Zipcode = zipcode,
                HouseNumber = houseNumber,
                ApartmentNumber = apartmentNumber
            };

            _addresses.Add(address);
            _uow.SaveChanges();
            return address;
        }


        public Address? Get(Guid id)
        {
            return _addresses.Get(id);
        }

        public IReadOnlyList<Address> FullAddr()
        {
            return new ReadOnlyCollection<Address>(_addresses.Query().ToList());
        }
    }
}
