using System.Threading.Tasks;
using BetSoftware.CustomExceptions;
using BetSoftware.DT0;
using BetSoftware.Model;
using BetSoftware.Utilities;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BetSoftware.Data
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticatedUser> SignIn(User user)
        {
            var dbUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (dbUser == null || _passwordHasher.VerifyHashedPassword(dbUser.Password,user.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid username or password");
            }

            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateUserToken(user.Username)
            };
        }

        public async Task<AuthenticatedUser> SignUp(User user)
        {
            var checkuser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.Equals(user.Username));

            if (checkuser != null)
            {
                throw new UsernameAlreadyExistsException("Username already exists in our BetSoftware DB");
            }

            user.Password = _passwordHasher.HashPassword(user.Password);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateUserToken(user.Username)
            };
        }
    }
}