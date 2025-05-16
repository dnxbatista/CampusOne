using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampusOneAPI.Models.Entities
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = "Unknown";

        [BsonElement("email")]
        [BsonRequired]
        public required string Email { get; set; }

        [BsonElement("number")]
        public string? Number { get; set; } = null;

        [BsonElement("usertype")]
        [BsonRepresentation(BsonType.String)]
        public required UserType UserType { get; set; } = UserType.Student;

        [BsonElement("passwordHash")]
        [BsonRequired]
        public required string PasswordHash { get; set; }
    }

    public enum UserType
    {
        Student,
        Teacher,
        Admin
    }
}
