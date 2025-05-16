using CampusOneAPI.Data;
using CampusOneAPI.Models;
using CampusOneAPI.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CampusOneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public UserController(MongoDbService mongoDbService)
        {
            _users = mongoDbService.Database.GetCollection<User>("users");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _users.Find(FilterDefinition<User>.Empty).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id); // Use string field name instead of lambda
            var user = await _users.Find(filter).FirstOrDefaultAsync();
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser([FromBody] LoginDto loginDto)
        {
            if (loginDto == null) return BadRequest("User data is null");
            var filter = Builders<User>.Filter.Eq("Email", loginDto.Email);
            var user = await _users.Find(filter).FirstOrDefaultAsync();
            if (user == null) return BadRequest("User not found");
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user?.PasswordHash);
            return isPasswordValid ? Ok(loginDto) : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null) return BadRequest("User data is null");

            await _users.InsertOneAsync(new User
            {
                Name = registerDto.Name == null ? "Unknown" : registerDto.Name,
                Email = registerDto.Email,
                Number = registerDto.Number == null ? String.Empty : registerDto.Number,
                UserType = registerDto.UserType,
                // Create a password hash based on the DTO password
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            });
            return CreatedAtAction(nameof(GetUserById), new { id = registerDto.Email }, registerDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null) return BadRequest("User data is null");
            var filter = Builders<User>.Filter.Eq("Id", user.Id);
            var update = Builders<User>.Update
                .Set(u => u.Name, user.Name)
                .Set(u => u.Email, user.Email)
                .Set(u => u.Number, user.Number)
                .Set(u => u.UserType, user.UserType)
                .Set(u => u.PasswordHash, BCrypt.Net.BCrypt.HashPassword(user.PasswordHash));
            var result = await _users.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0 ? Ok(user) : NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var result = await _users.DeleteOneAsync(filter);
            return result.DeletedCount > 0 ? Ok() : NotFound();
        }
    }
}
