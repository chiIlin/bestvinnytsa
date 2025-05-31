using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly AppDbContext _db;
        public ApplicationService(AppDbContext db) => _db = db;

        public async Task<List<Application>> GetByCampaignAsync(int campaignId)
        {
            return await _db.Applications
                .Include(a => a.Influencer)
                .Where(a => a.CampaignId == campaignId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Application>> GetByInfluencerAsync(int influencerId)
        {
            return await _db.Applications
                .Include(a => a.Campaign)
                .ThenInclude(c => c.Producer)
                .Where(a => a.InfluencerId == influencerId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task ApplyAsync(Application newApplication)
        {
            _db.Applications.Add(newApplication);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int applicationId, ApplicationStatus status)
        {
            var app = await _db.Applications.FindAsync(applicationId);
            if (app != null)
            {
                app.Status = status;
                await _db.SaveChangesAsync();
            }
        }
    }
}