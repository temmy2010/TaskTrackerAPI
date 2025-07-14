using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackerAPI.Domain.Entities;

namespace TaskTrackerAPI.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsername(string username);
        Task SaveChangesAsync();
    }
}
