using MG_Project.DataModel;

namespace MG_Project.Abstractions
{
    public interface IAddressRepositories
    {
        IQueryable<Address> Query();
        Address? Get(Guid id);
        void Add(Address entity);
        void Remove(Address entity);
    }

}
