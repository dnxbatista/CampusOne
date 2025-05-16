using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CampusOneAPI.Models.Entities
{
    public class Task
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public required string Title {  get; set; }

        public string? Description { get; set; }

        public DateTime? Deadline { get; set; }

    }
}
