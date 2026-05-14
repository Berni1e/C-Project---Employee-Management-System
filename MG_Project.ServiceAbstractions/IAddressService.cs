using MG_Project.DataModel;

public interface IAddressService
{
    Address AddAddress(string street, string city, string country, string zipcode, int houseNumber, int? apartmentNumber);
    Address? Get(Guid id);
    IReadOnlyList<Address> FullAddr();
}
