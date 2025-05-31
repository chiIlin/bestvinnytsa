using System.Threading.Tasks;
using bestvinnytsa.web.Data.DTOs;

namespace bestvinnytsa.web.Data.Services
{
    /// <summary>
    /// Сервіс для реєстрації (Register) та входу (Login) користувача. 
    /// Після успішної реєстрації/логіну повертає JWT-токен.
    /// </summary>
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
    }
}
