using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using srcds_control_api.Exceptions.User;
using srcds_control_api.Models;
using srcds_control_api.Models.DTOs;
using srcds_control_api.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace srcds_control_api.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<MongoUser> _users;
        private readonly IPasswordEncryptor _encryptor;
        private readonly IConfiguration _config;

        public UserService(IConfiguration config, IPasswordEncryptor encryptor)
        {
            var client = new MongoClient(config.GetConnectionString("SrcdControlDb"));
            var database = client.GetDatabase("SrcdControl");
            _users = database.GetCollection<MongoUser>("users");

            _encryptor = encryptor;
            _config = config;
        }

        public async Task<User> AuthenticateUser(User user)
        {
            try
            {
                var mongoUser = (await _users.FindAsync(u => u.UserName == user.UserName && u.Enabled == true)).FirstOrDefault();

                if (mongoUser == null)
                {
                    throw new UserNotFoundException(user.UserName);
                }

                if(!_encryptor.TryMatchPassword(new EncryptedPassword(mongoUser.Salt, mongoUser.HashedPassword), user.Password))
                {
                    throw new WrongPasswordException(user.UserName);
                }

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetSection("JWT").GetValue<string>("secret"));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, mongoUser.Id.ToString()),
                        new Claim(ClaimTypes.Role, mongoUser.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new User()
                {
                    UserName = user.UserName,
                    Password = null,
                    Role = mongoUser.Role,
                    Token = tokenHandler.WriteToken(token)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ChangePassword(PasswordChangeDto dto)
        {
            try
            {
                var user = (await _users.FindAsync(u => u.UserName == dto.UserName && u.Enabled == true)).FirstOrDefault();

                if (user == null)
                {
                    throw new UserNotFoundException(dto.UserName);
                }

                if (!_encryptor.TryMatchPassword(new EncryptedPassword(user.Salt, user.HashedPassword), dto.CurrentPassword))
                {
                    throw new WrongPasswordException(dto.UserName);
                }

                var encrypted = _encryptor.CreateUserPassword(dto.NewPassword);

                user.HashedPassword = Convert.ToBase64String(encrypted.HashedPassword);
                user.Salt = Convert.ToBase64String(encrypted.Salt);

               await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateUser(User user)
        {
            try
            {
                var checkUser = (await _users.FindAsync(u => u.UserName == user.UserName && u.Enabled == true)).FirstOrDefault();

                if(checkUser != null)
                {
                    throw new UserAlreadyExistsException(user.UserName);
                }

                var encrypted = _encryptor.CreateUserPassword(user.Password);

                var mongoUser = new MongoUser()
                {
                    UserName = user.UserName,
                    HashedPassword = Convert.ToBase64String(encrypted.HashedPassword),
                    Salt = Convert.ToBase64String(encrypted.Salt),
                    Role = user.Role,
                    Enabled = true
                };

                await _users.InsertOneAsync(mongoUser);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
