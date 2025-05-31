using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace bestvinnytsa.web.Data.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly AppDbContext _db;
        public CampaignService(AppDbContext db) => _db = db;

        public async Task<List<Campaign>> GetOpenCampaignsAsync()
        {
            return await _db.Campaigns
                .Where(c => c.IsOpen)
                .Include(c => c.Category)
                .Include(c => c.Producer)
                .ToListAsync();
        }

        public async Task<Campaign?> GetByIdAsync(int id)
        {
            return await _db.Campaigns
                .Include(c => c.Category)
                .Include(c => c.Producer)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Campaign>> GetByProducerAsync(string producerId)
        {
            return await _db.Campaigns
                .Where(c => c.ProducerId == producerId)
                .Include(c => c.Category)
                .Include(c => c.Producer)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task CreateAsync(Campaign newCampaign)
        {
            _db.Campaigns.Add(newCampaign);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Campaign updatedCampaign)
        {
            _db.Campaigns.Update(updatedCampaign);
            await _db.SaveChangesAsync();
        }

        public async Task CloseAsync(int campaignId)
        {
            var campaign = await _db.Campaigns.FindAsync(campaignId);
            if (campaign != null)
            {
                campaign.IsOpen = false;
                await _db.SaveChangesAsync();
            }
        }
    }
}
