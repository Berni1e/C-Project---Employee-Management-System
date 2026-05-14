using MG_Project.DataModel;

namespace MG_Project.Abstractions
{
    public interface IPersonRepositories
    {
        IQueryable<Person> Query();
        Person? Get(Guid id);
        void Add(Person entity);
        void Remove(Person entity);
    }

}
