using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MG_Project.DataModel;

namespace MG_Project.Abstractions
{
    public interface IFirmRepositories
    {
        IQueryable<Firm> Query();
        Firm? Get(Guid id);
        void Add(Firm entity);
        void Remove(Firm entity);
    }
}
