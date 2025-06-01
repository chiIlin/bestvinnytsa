using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    /// <summary>
    /// Сервіс для CRUD-операцій над колекцією Campaigns,
    /// а також для отримання списку категорій.
    /// </summary>
    public interface ICampaignService
    {
        Task<List<Campaign>> GetOpenCampaignsAsync();
        Task<Campaign?> GetByIdAsync(string id);
        Task<List<Campaign>> GetByProducerAsync(string producerId);
        Task<List<Category>> GetCategoriesAsync();
        Task CreateAsync(Campaign newCampaign);
        Task UpdateAsync(Campaign updatedCampaign);
        Task CloseAsync(string campaignId);
        Task DeleteAsync(string campaignId);
    }
}
