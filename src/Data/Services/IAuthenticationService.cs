using System.Threading.Tasks;
using bestvinnytsa.web.Data.DTOs;

namespace bestvinnytsa.web.Data.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAndGenerateTokenAsync(RegisterRequest request);
        Task<string> LoginAndGenerateTokenAsync(LoginRequest request);
    }
}