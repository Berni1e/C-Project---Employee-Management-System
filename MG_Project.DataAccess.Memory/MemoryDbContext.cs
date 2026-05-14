using MG_Project.DataModel;

namespace MG_Project.DataAccess.Memory
{
    public class MemoryDbContext
    {
        public List<Address> Address { get; } = new();
        public List<Person> Person { get; } = new();
        public List<Employee> Employee { get; } = new();
        public List<Firm> Firm { get; } = new();
        public List<Department> Department { get; } = new();
        public List<Tasks> Tasks { get; } = new();

        public int SaveChanges() => 0;
    }
}
