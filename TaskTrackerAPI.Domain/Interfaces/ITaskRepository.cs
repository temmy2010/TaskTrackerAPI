using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackerAPI.Domain.Entities;

namespace TaskTrackerAPI.Domain.Interfaces
{
    public interface ITaskRepository : IGenericRepository<TaskItem>
    {
        Task<int> GetTotalTaskCount(string? status = null);
        Task<int> GetCompletedTasks();
        Task<IEnumerable<TaskItem>> GetPagedTasks(int pageNumber, int pageSize, string? status);
        Task<int> GetTotalTasks();
        Task SaveChangesAsync();
    }

}
