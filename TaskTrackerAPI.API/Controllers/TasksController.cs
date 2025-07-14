using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TaskTrackerAPI.Application.DTOs;
using TaskTrackerAPI.Domain.Entities;
using TaskTrackerAPI.Domain.Interfaces;

/// <summary>
/// Manage tasks (CRUD, report)
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepo;
    private readonly IUserRepository _userRepo;

    public TasksController(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepo = taskRepository;
        _userRepo = userRepository;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskRepo.GetAll();
        return Ok(tasks);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10, string? status = null)
    {
        var pagedTasks = await _taskRepo.GetPagedTasks(pageNumber, pageSize, status);
        return Ok(pagedTasks);
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    [HttpPost("task")]
    public async Task<IActionResult> Create(TaskItemModel task)
    {
        var entity = new TaskItem
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = task.CompletedAt,
            Status = task.Status,
            AssignedToUserId = task.AssignedToUserId,  
        };
        await _taskRepo.Add(entity);
        await _taskRepo.SaveChangesAsync();
        return Ok(new { message = "Task created successfully.", entity });
    }

    /// <summary>
    /// Update task
    /// </summary>
    [HttpPut("task/{id}")]
    public async Task<IActionResult> Update(Guid id, TaskItemModel updateTask)
    {
        var task = await _taskRepo.GetById(id);
        if (task == null) return NotFound();

        task.Title = updateTask.Title;
        task.Description = updateTask.Description;
        task.Status = updateTask.Status;
        task.DueDate = updateTask.DueDate;
        task.CompletedAt = updateTask.CompletedAt;
        task.AssignedToUserId = updateTask.AssignedToUserId;

        _taskRepo.Update(task);
        await _taskRepo.SaveChangesAsync();
        return Ok(new { message = "Task updated successfully.", task });
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    [HttpDelete("task/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var task = await _taskRepo.GetById(id);
        if (task == null) return NotFound();

        _taskRepo.Delete(task);
        await _taskRepo.SaveChangesAsync();
        return Ok(new { message = "Task deleted successfully." });
    }

    /// <summary>
    /// Update task Status to Completed
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> MarkComplete(Guid id)
    {
        var task = await _taskRepo.GetById(id);
        if (task == null) return NotFound();

        task.Status = "Completed";
        task.CompletedAt = DateTime.UtcNow;

        _taskRepo.Update(task);
        await _taskRepo.SaveChangesAsync();
        return Ok(task);
    }

    /// <summary>
    /// Get completion report (only for Managers)
    /// </summary>
    [Authorize(Roles = "Manager,manager,MANAGER")]
    [HttpGet("/api/reports/completion")]
    public async Task<IActionResult> GetReport()
    {
        var total = await _taskRepo.GetTotalTaskCount();
        var completed = await _taskRepo.GetCompletedTasks();
        var rate = total == 0 ? 0 : ((double)completed / total) * 100;

        return Ok(new
        {
            totalTasks = total,
            completedTasks = completed,
            completionRate = rate
        });
    }
}
