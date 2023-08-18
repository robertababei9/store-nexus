using Application.Repositories.Contracts;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {

        ApplicationContext GetContext();
        void Dispose();
    }
}
