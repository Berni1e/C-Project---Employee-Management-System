using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.DataModel
{
    public class Address
    {
        public Guid AddressId {  get; set; }
        public string? Street {  get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Zipcode { get; set; }
        public int HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        
        public Address()
        {
            AddressId = Guid.NewGuid();
            Street = string.Empty;
            City = string.Empty;
            Country = string.Empty;
            Zipcode = string.Empty;
            HouseNumber = 0;
            ApartmentNumber = null;
        }

        public Address(string? street, string? city, string? country, string? zipcode, int houseNumber, int? apartmentNumber)
        {
            Street = street;
            City = city;
            Country = country;
            Zipcode = zipcode;
            HouseNumber = houseNumber;
            ApartmentNumber = apartmentNumber;
        }

        public string FullAddr()
        {
            string address = $"{Street} {HouseNumber}";

            if (ApartmentNumber.HasValue)
            {
                address += $"/{ApartmentNumber.Value}";
            }

            address += $", {Zipcode} {City}, {Country}";

            return address;
        }

        public override string ToString()
        {
            return FullAddr();
        }
    }
}
