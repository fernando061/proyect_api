using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorck;
public class UnitOfWork : IUnitOfWork
{
    private readonly TiendaContext _context;
    private IUserRepository _user;
    private IProductRepository _product;

    public UnitOfWork(TiendaContext context)
    {
        _context = context;
    }



    public IUserRepository User
    {
        get
        {
            if (_user == null)
            {
                _user = new UserRepository(_context);
            }
            return _user;
        }
    }
    public IProductRepository Product
    {
        get
        {
            if (_product == null)
            {
                _product = new ProductRepository(_context);
            }
            return _product;
        }
    }

    public void BeginTransaction()
    {
        _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        //inicia una nueva transaccion y guarda _context.Database.BeginTransaction().Commit();
        _context.Database.CommitTransaction();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public void Rollback()
    {
        //inicia una nueva transaccion y guarda _context.Database.BeginTransaction().Commit();
        _context.Database.RollbackTransaction();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

