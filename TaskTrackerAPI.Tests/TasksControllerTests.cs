using Xunit;
using Moq;
using TaskTrackerAPI.API.Controllers;
using TaskTrackerAPI.Domain.Interfaces;
using TaskTrackerAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Application.DTOs;

public class TasksControllerTests
{
    private readonly TasksController _controller;
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;

    public TasksControllerTests()
    {
        _taskRepoMock = new Mock<ITaskRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _controller = new TasksController(_taskRepoMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult()
    {
        _taskRepoMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<TaskItem>());

        var result = await _controller.GetAll();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnOkResult()
    {
        var newTask = new TaskItemModel { Title = "Test", Description = "Desc" };

        var result = await _controller.Create(newTask);

        Assert.IsType<OkObjectResult>(result);
        _taskRepoMock.Verify(r => r.Add(It.IsAny<TaskItem>()), Times.Once);
        _taskRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
