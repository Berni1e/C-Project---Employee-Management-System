using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Project.Abstractions
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}
