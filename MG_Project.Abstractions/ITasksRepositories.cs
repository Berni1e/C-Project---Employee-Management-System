using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.DataModel;

namespace MG_Project.Abstractions
{
    public interface ITasksRepositories
    {
        IQueryable<Tasks> Query();
        Tasks? Get(Guid id);
        void Add(Tasks entity);
        void Remove(Tasks entity);
    }
}
