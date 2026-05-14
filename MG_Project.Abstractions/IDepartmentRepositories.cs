using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.DataModel;

namespace MG_Project.Abstractions
{
    public interface IDepartmentRepositories
    {
        IQueryable<Department> Query();
        Department? Get(Guid id);
        void Add(Department entity);
        void Remove(Department entity);
    }
}
