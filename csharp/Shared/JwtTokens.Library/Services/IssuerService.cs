using JwtAuth.Library.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtAuth.Library.Services
{
    public interface IIssuerService
    {
        (byte[] passwordSalt, byte[] passwordHash) CreatePasswordHash(string password);
        string CreateToken(User user);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }

    public class IssuerService : IIssuerService
    {
        public virtual string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "admin")
        };

            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes("GetTheKeyFromTheConfigurationAfterExtraction"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                "Server",
                "Client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        public virtual bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public virtual (byte[] passwordSalt, byte[] passwordHash) CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            return (hmac.Key, hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
    }
}
