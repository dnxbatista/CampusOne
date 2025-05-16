using CampusOneAPI.Models.Entities;
using MongoDB.Driver;

namespace CampusOneAPI.Data
{
    public class MongoDbService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            _configuration = configuration;
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                var mongoUrl = MongoUrl.Create(connectionString);
                var mongoClient = new MongoClient(connectionString);
                _database = mongoClient.GetDatabase("CampusOneDB");
            }
            catch (Exception ex)
            {
                throw new Exception($"MongoDB connection failed: {ex.Message}", ex);
            }        
        }

        public IMongoDatabase? Database => _database;
    }
}
