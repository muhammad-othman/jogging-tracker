using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Enums;
using JoggingTrackerServer.Domain.IRepositories;
using JoggingTrackerServer.Persistence.Shared;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JoggingTrackerServer.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IJoggingDBContext database) : base(database)
        {

        }
        public User AuthorizateUser(string Token)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Token == Token && u.TokenExpiration > DateTime.Now);
        }

        public LoginResult Login(string UserName, string Password, bool Remmber, out string Token, out User user)
        {
            Token = String.Empty;
            user = null;
            user = _dbContext.Users.FirstOrDefault(u => u.UserName == UserName);
            if (user == null)
                return LoginResult.UserNotFound;
            else if (user.Password != Password)
                return LoginResult.WrongPassword;

            user.Token = Token = GetUniqueKey(25);

            if (Remmber)
                user.TokenExpiration = DateTime.Now.AddDays(30);
            else
                user.TokenExpiration = DateTime.Now.AddMinutes(15);
            _dbContext.Save();
            return LoginResult.LoginSuccessfull;

        }

        public bool RefreshToken(string Token)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Token == Token);
            if (user == null) return false;

            var futureDate = DateTime.Now.AddMinutes(15);
            if (futureDate > user.TokenExpiration)
            {
                user.TokenExpiration = futureDate;
                _dbContext.Entry<User>(user).Property(u => u.TokenExpiration).IsModified = true;
                _dbContext.Save();
            }
            return true;
        }
        public bool Logout(string Token)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Token == Token);
            if (user == null) return false;

            user.TokenExpiration = DateTime.Now.AddMinutes(-15);
            _dbContext.Entry<User>(user).Property(u => u.TokenExpiration).IsModified = true;
            _dbContext.Save();
            return true;
        }
        private static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

    }
}
