using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Mongo;
using bestvinnytsa.web.Data.Models;
using MongoDB.Driver;

namespace bestvinnytsa.web.Data.Services
{
    /// <summary>
    /// Реалізація сервісу CampaignService для MongoDB.
    /// </summary>
    public class CampaignService : ICampaignService
    {
        private readonly IMongoCollection<Campaign> _campaigns;
        private readonly IMongoCollection<Category> _categories;

        public CampaignService(MongoContext mongoContext)
        {
            _campaigns = mongoContext.Campaigns;
            _categories = mongoContext.Categories;
        }

        /// <summary>
        /// Повертає всі кампанії з IsOpen == true.
        /// </summary>
        public async Task<List<Campaign>> GetOpenCampaignsAsync()
        {
            return await _campaigns
                .Find(c => c.IsOpen)
                .ToListAsync();
        }

        /// <summary>
        /// Повертає кампанію за ObjectId (string).
        /// </summary>
        public async Task<Campaign?> GetByIdAsync(string id)
        {
            return await _campaigns
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Повертає всі кампанії даного продюсера (ProducerId == userId).
        /// </summary>
        public async Task<List<Campaign>> GetByProducerAsync(string producerId)
        {
            return await _campaigns
                .Find(c => c.ProducerId == producerId)
                .ToListAsync();
        }

        /// <summary>
        /// Повертає всі категорії.
        /// </summary>
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Створює нову кампанію:
        /// - Встановлює CreatedAt = UtcNow
        /// - Встановлює IsOpen = true
        /// - Вставляє документ у колекцію Campaigns
        /// </summary>
        public async Task CreateAsync(Campaign newCampaign)
        {
            // Перевіряємо чи встановлено ProducerId
            if (string.IsNullOrEmpty(newCampaign.ProducerId))
            {
                throw new ArgumentException("ProducerId is required");
            }

            // MongoDB автоматично створить Id, якщо його нема
            if (string.IsNullOrEmpty(newCampaign.Id))
            {
                newCampaign.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            }
            
            newCampaign.CreatedAt = DateTime.UtcNow;
            newCampaign.IsOpen = true;
            
            await _campaigns.InsertOneAsync(newCampaign);
        }

        /// <summary>
        /// Оновлює існуючу кампанію повністю (ReplaceOne).
        /// </summary>
        public async Task UpdateAsync(Campaign updatedCampaign)
        {
            var filter = Builders<Campaign>.Filter.Eq(c => c.Id, updatedCampaign.Id);
            await _campaigns.ReplaceOneAsync(filter, updatedCampaign);
        }

        /// <summary>
        /// Закриває кампанію (IsOpen = false).
        /// </summary>
        public async Task CloseAsync(string campaignId)
        {
            var filter = Builders<Campaign>.Filter.Eq(c => c.Id, campaignId);
            var update = Builders<Campaign>.Update.Set(c => c.IsOpen, false);
            await _campaigns.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Видаляє кампанію з колекції (DeleteOne).
        /// </summary>
        public async Task DeleteAsync(string campaignId)
        {
            var filter = Builders<Campaign>.Filter.Eq(c => c.Id, campaignId);
            await _campaigns.DeleteOneAsync(filter);
        }
    }
}
