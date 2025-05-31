using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    public interface ICampaignService
    {
        Task<List<Campaign>> GetOpenCampaignsAsync();
        Task<Campaign?> GetByIdAsync(int id);
        Task CreateAsync(Campaign newCampaign);
        Task UpdateAsync(Campaign campaign);
        Task CloseAsync(int id);
        Task<List<Campaign>> GetByProducerAsync(int producerId);
        Task<List<Category>> GetAllCategoriesAsync();
    }
}