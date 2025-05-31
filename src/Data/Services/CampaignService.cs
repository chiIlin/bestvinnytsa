using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly AppDbContext _db;
        public CampaignService(AppDbContext db) => _db = db;

        public async Task<List<Campaign>> GetOpenCampaignsAsync()
        {
            return await _db.Campaigns
                .Include(c => c.Producer)
                .Include(c => c.Category)
                .Where(c => c.IsOpen && (c.ExpiresAt == null || c.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Campaign?> GetByIdAsync(int id)
        {
            return await _db.Campaigns
                .Include(c => c.Producer)
                .Include(c => c.Category)
                .Include(c => c.Applications)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Campaign newCampaign)
        {
            _db.Campaigns.Add(newCampaign);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Campaign campaign)
        {
            _db.Campaigns.Update(campaign);
            await _db.SaveChangesAsync();
        }

        public async Task CloseAsync(int id)
        {
            var campaign = await _db.Campaigns.FindAsync(id);
            if (campaign != null)
            {
                campaign.IsOpen = false;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Campaign>> GetByProducerAsync(int producerId)
        {
            return await _db.Campaigns
                .Include(c => c.Category)
                .Where(c => c.ProducerId == producerId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
