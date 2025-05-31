using MongoDB.Driver;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Mongo
{
	/// <summary>
	/// “Контекст” для Mongo: дає доступ до колекцій.
	/// </summary>
	public class MongoContext
	{
		private readonly IMongoDatabase _database;

		public MongoContext(IMongoDatabase database)
		{
			_database = database;
		}

		public IMongoCollection<AppUser> Users => _database.GetCollection<AppUser>("Users");
		public IMongoCollection<AppRole> Roles => _database.GetCollection<AppRole>("Roles");
		public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
		public IMongoCollection<Campaign> Campaigns => _database.GetCollection<Campaign>("Campaigns");
		public IMongoCollection<Application> Applications => _database.GetCollection<Application>("Applications");
	}
}

