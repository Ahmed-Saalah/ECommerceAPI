using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace eCommerceApp.Infrastructure.Repositories.Authentication
{
    public class TokenManagement(AppDbContext context, IConfiguration config) : ITokenManagement
    {
        public async Task<int> AddRefreshToken(string UserId, string RefreshToken)
        {
            context.RefreshTokens.Add(new RefreshToken
            {
                UserId = UserId,
                Token = RefreshToken
            });
            return await context.SaveChangesAsync();
        }

        public string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(2);
            var token = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetRefreshToken()
        {
            const int byteSize = 64;
            byte[] randomBytes = new byte[byteSize];

            using (RandomNumberGenerator rnd = RandomNumberGenerator.Create()) 
            {
                rnd.GetBytes(randomBytes);
            }

            string token = Convert.ToBase64String(randomBytes);
            return WebUtility.UrlEncode(token);
        }

        public List<Claim> GetUserClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //var jwtToken = tokenHandler.ReadToken(token);
            JwtSecurityToken? jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken != null)
                return jwtToken.Claims.ToList();
            else
                return [];
        }

        public async Task<string> GetUserIdByRefreshToken(string refreshToken)
            => (await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken))!.UserId;

        public async Task<int> UpdateRefreshToken(string UserId, string refreshToken)
        {
            var user = await context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);
            
            if (user == null)
                return -1;

            user.Token = refreshToken;
            return await context.SaveChangesAsync();
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var user = await context.RefreshTokens
                .FirstOrDefaultAsync (x => x.Token == refreshToken);
            
            return user != null;
        }
    }
}
