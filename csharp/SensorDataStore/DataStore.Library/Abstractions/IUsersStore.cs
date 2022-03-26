using JwtAuth.Library.Models;
using System.Threading.Tasks;

namespace DataStore.Library.Abstractions
{
    public interface IUsersStore
    {
        Task<bool> UserExistsAsync(string username);
        ValueTask<User> TryGetUser(string username);
        Task SaveUser(User user);
    }
}
