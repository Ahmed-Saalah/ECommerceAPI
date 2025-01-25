using System.Security.Claims;

namespace eCommerceApp.Domain.Interfaces.Authentication
{
    public interface ITokenManagement
    {
        string GetRefreshToken();
        List<Claim> GetUserClaimsFromToken(string token);
        Task<bool> ValidateRefreshToken(string refreshToken);   
        Task<string> GetUserIdByRefreshToken(string refreshToken);  
        Task<int> AddRefreshToken(string UserId, string refreshToken);
        Task<int> UpdateRefreshToken(string UserId, string RefreshToken);
        string GenerateToken(List<Claim> claims);
    }
}
