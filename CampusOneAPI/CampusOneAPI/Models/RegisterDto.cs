using CampusOneAPI.Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace CampusOneAPI.Models
{
    public class RegisterDto
    {
        public string? Name { get; set; } = "Unknown";

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        public string? Number { get; set; } = null;
        [BsonRepresentation(BsonType.String)]
        public UserType UserType { get; set; } = UserType.Student;
    }
}
