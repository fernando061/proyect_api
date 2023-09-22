using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;
public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(TiendaContext context) : base(context)
    {
    }

    public async Task<User> GetByUsernameAsync(string email)
    {
        return await _context.User
                            //.Include(u => u.Roles)
                            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}

