using System.Threading.Tasks;
using BetSoftware.DT0;
using BetSoftware.Model;

namespace BetSoftware.Data
{
    public interface IUserService
    {
        Task<AuthenticatedUser> SignUp(User user);
         Task<AuthenticatedUser> SignIn(User user);
    }
}