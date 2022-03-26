using Couchbase.KeyValue;
using DataStore.Library.Abstractions;
using JwtAuth.Library.Models;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace DataStore.Library.Data
{
    public class UsersStore : IUsersStore
    {
        private readonly ICouchbaseDataAccess _couchbaseDataAccess;
        public UsersStore(ICouchbaseDataAccess couchbaseDataAccess)
        {
            _couchbaseDataAccess = couchbaseDataAccess;
        }

        public Task SaveUser(User user)
        {
            return _couchbaseDataAccess.APIUsersData.InsertAsync(Guid.NewGuid().ToString(), user,
             new InsertOptions());
        }

        public ValueTask<User> TryGetUser(string username)
        {
            var query = $"SELECT userName, passwordHash, passwordSalt FROM MinimalApiUserDb._default.APIUsersData where userName = '{username}' ";
            var user = _couchbaseDataAccess.LoadDataAsync<User>(query).Result;

            var userData = user.Select(u => new User()
            {
                UserName = u.UserName,
                PasswordHash = u.PasswordHash,
                PasswordSalt = u.PasswordSalt
            }).FirstOrDefaultAsync();
            return userData;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return (await TryGetUser(username) != null);
        }
    }
}
