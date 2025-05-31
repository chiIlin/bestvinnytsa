using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    public interface IApplicationService
    {
        Task<List<Application>> GetByCampaignAsync(int campaignId);
        Task<List<Application>> GetByInfluencerAsync(string influencerId);
        Task ApplyAsync(Application newApplication);
        Task UpdateStatusAsync(int applicationId, ApplicationStatus status);
    }
}
