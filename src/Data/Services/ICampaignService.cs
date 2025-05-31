using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    public interface ICampaignService
    {
        Task<List<Campaign>> GetOpenCampaignsAsync();
        Task<Campaign?> GetByIdAsync(int id);
        Task<List<Campaign>> GetByProducerAsync(string producerId);
        Task<List<Category>> GetCategoriesAsync();
        Task CreateAsync(Campaign newCampaign);
        Task UpdateAsync(Campaign updatedCampaign);
        Task CloseAsync(int campaignId);
    }
}
