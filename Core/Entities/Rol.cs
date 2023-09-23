using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
    public class Rol
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> User { get; set; } = new HashSet<User>();
        public ICollection<UsersRoles> UsersRoles { get; set; }
    }

