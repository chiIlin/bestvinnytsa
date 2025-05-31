using System.Threading.Tasks;
using bestvinnytsa.web.Data.DTOs;

namespace bestvinnytsa.web.Data.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterPersonAsync(PersonRegisterRequest request);
        Task<string> RegisterCompanyAsync(CompanyRegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
    }
}
