using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Mongo;
using bestvinnytsa.web.Data.Models;
using MongoDB.Driver;

namespace bestvinnytsa.web.Data.Services
{
    /// <summary>
    /// Реалізація сервісу ApplicationService для MongoDB.
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        private readonly IMongoCollection<Application> _applications;

        public ApplicationService(MongoContext mongoContext)
        {
            _applications = mongoContext.Applications;
        }

        /// <summary>
        /// Повертає всі заявки, де InfluencerId == userId.
        /// </summary>
        public async Task<List<Application>> GetByInfluencerAsync(string influencerId)
        {
            return await _applications
                .Find(a => a.InfluencerId == influencerId)
                .ToListAsync();
        }

        /// <summary>
        /// Повертає всі заявки до певної кампанії (CampaignId == campaignId).
        /// </summary>
        public async Task<List<Application>> GetByCampaignAsync(string campaignId)
        {
            return await _applications
                .Find(a => a.CampaignId == campaignId)
                .ToListAsync();
        }

        /// <summary>
        /// Повертає заявку за ObjectId (string).
        /// </summary>
        public async Task<Application?> GetByIdAsync(string id)
        {
            return await _applications
                .Find(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Створює нову заявку:
        /// - Встановлює CreatedAt = UtcNow
        /// - Встановлює Status = "Pending"
        /// - Вставляє документ у колекцію Applications
        /// </summary>
        public async Task CreateAsync(Application newApplication)
        {
            newApplication.CreatedAt = DateTime.UtcNow;
            newApplication.Status = "Pending";
            await _applications.InsertOneAsync(newApplication);
        }

        /// <summary>
        /// Повністю оновлює документ заявки (ReplaceOne).
        /// </summary>
        public async Task UpdateAsync(Application updatedApplication)
        {
            var filter = Builders<Application>.Filter.Eq(a => a.Id, updatedApplication.Id);
            await _applications.ReplaceOneAsync(filter, updatedApplication);
        }

        /// <summary>
        /// Змінює лише поле Status (наприклад, "Accepted", "Rejected").
        /// </summary>
        public async Task SetStatusAsync(string applicationId, string status)
        {
            var filter = Builders<Application>.Filter.Eq(a => a.Id, applicationId);
            var update = Builders<Application>.Update.Set(a => a.Status, status);
            await _applications.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Видаляє заявку (DeleteOne).
        /// </summary>
        public async Task DeleteAsync(string applicationId)
        {
            var filter = Builders<Application>.Filter.Eq(a => a.Id, applicationId);
            await _applications.DeleteOneAsync(filter);
        }
    }
}
