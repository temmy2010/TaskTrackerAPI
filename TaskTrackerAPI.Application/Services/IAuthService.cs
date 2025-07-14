using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackerAPI.Application.DTOs;

namespace TaskTrackerAPI.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> Login(LoginModel model);
        Task<bool> Register(RegisterModel model);
    }
}
