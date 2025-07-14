using Microsoft.Extensions.Configuration;
using Moq;
using TaskTrackerAPI.Application.DTOs;
using TaskTrackerAPI.Application.Services;
using TaskTrackerAPI.Domain.Entities;
using TaskTrackerAPI.Domain.Interfaces;
using Xunit;
using Microsoft.Extensions.Configuration;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();

        var settings = new Dictionary<string, string> {
            {"Jwt:Key", "A7zA!3w5g7Q@ZpLx#VkT6%RmBcY1!Xhg"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"}
        };
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        _authService = new AuthService(_userRepoMock.Object, _config);
    }

    [Fact]
    public async Task Register_ReturnsFalse_WhenUserExists()
    {
        _userRepoMock.Setup(r => r.GetByUsername("existing")).ReturnsAsync(new User());

        var result = await _authService.Register(new RegisterModel { Username = "existing", Password = "pwd", Role = "User" });

        Assert.False(result);
    }

    [Fact]
    public async Task Register_ReturnsTrue_WhenUserDoesNotExist()
    {
        _userRepoMock.Setup(r => r.GetByUsername("new")).ReturnsAsync((User?)null);

        var result = await _authService.Register(new RegisterModel { Username = "new", Password = "pwd", Role = "User" });

        Assert.True(result);
        _userRepoMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenUserNotFound()
    {
        _userRepoMock.Setup(r => r.GetByUsername("notfound")).ReturnsAsync((User?)null);

        var res = await _authService.Login(new LoginModel { Username = "notfound", Password = "pwd" });

        Assert.Null(res);
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsAreCorrect()
    {
        var user = new User
        {
            Username = "test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pwd"),
            Role = "User"
        };
        _userRepoMock.Setup(r => r.GetByUsername("test")).ReturnsAsync(user);

        var res = await _authService.Login(new LoginModel { Username = "test", Password = "pwd" });

        Assert.NotNull(res);
        Assert.Equal("test", res.Username);
    }
}
