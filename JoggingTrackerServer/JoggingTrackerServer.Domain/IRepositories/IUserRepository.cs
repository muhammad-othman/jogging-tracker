using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Enums;
using JoggingTrackerServer.Domain.Interfaces;

namespace JoggingTrackerServer.Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        LoginResult Login(string UserName, string Password, bool Remmber, out string Token, out User user);
        bool Logout(string Token);
        bool RefreshToken(string Token);
        User AuthorizateUser(string Token);
    }
}
