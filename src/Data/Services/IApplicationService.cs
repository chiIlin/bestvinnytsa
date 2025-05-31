using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    /// <summary>
    /// Сервіс для CRUD-операцій над колекцією Applications.
    /// </summary>
    public interface IApplicationService
    {
        Task<List<Application>> GetByInfluencerAsync(string influencerId);
        Task<List<Application>> GetByCampaignAsync(string campaignId);
        Task<Application?> GetByIdAsync(string id);
        Task CreateAsync(Application newApplication);
        Task UpdateAsync(Application updatedApplication);
        Task SetStatusAsync(string applicationId, string status);
        Task DeleteAsync(string applicationId);
    }
}
