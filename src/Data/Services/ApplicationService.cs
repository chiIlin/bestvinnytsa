using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace bestvinnytsa.web.Data.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly AppDbContext _db;
        public ApplicationService(AppDbContext db) => _db = db;

        public async Task<List<Application>> GetByCampaignAsync(int campaignId)
        {
            return await _db.Applications
                .Where(a => a.CampaignId == campaignId)
                .Include(a => a.Influencer)
                .ToListAsync();
        }

        public async Task<List<Application>> GetByInfluencerAsync(string influencerId)
        {
            return await _db.Applications
                .Where(a => a.InfluencerId == influencerId)
                .Include(a => a.Campaign)
                    .ThenInclude(c => c.Producer)
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
