using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Domain.Entities;
using TaskTrackerAPI.Domain.Interfaces;
using TaskTrackerAPI.Infrastructure.Data;

namespace TaskTrackerAPI.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }

        public async Task<int> GetCompletedTasks() =>
            await _dbSet.CountAsync(t => t.Status == "Completed");

        public async Task<int> GetTotalTasks() => await _dbSet.CountAsync();

        public async Task<int> GetTotalTaskCount(string? status = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);
            return await query.CountAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetPagedTasks(int pageNumber, int pageSize, string? status = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
