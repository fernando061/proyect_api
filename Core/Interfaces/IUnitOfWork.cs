using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;
public interface IUnitOfWork
{
    IProductRepository Product { get; }
    IUserRepository User { get; }
    Task<int> SaveAsync();
    void Dispose();

    void BeginTransaction();
    void Commit();
    void Rollback();
}
