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
    public class TaskController : ControllerBase
    {
        private readonly IMongoCollection<Models.Entities.Task> _tasks;

        public TaskController(MongoDbService mongoDbService)
        {
            _tasks = mongoDbService.Database.GetCollection<Models.Entities.Task>("tasks");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _tasks.Find(FilterDefinition<Models.Entities.Task>.Empty).ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Entities.Task?>> GetTaskById(string id)
        {
            var filter = Builders<Models.Entities.Task>.Filter.Eq("Id", id);
            var task = await _tasks.Find(filter).FirstOrDefaultAsync();
            return task != null ? Ok(task) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] EditTaskDto editTaskDto)
        {
            if (editTaskDto == null) return BadRequest("Null Body");
            //var task = await _tasks.Find(editTaskDto.Title).FirstOrDefaultAsync();
            //if (task == null) return BadRequest("Title Already Being Used");

            await _tasks.InsertOneAsync(new Models.Entities.Task
            {
                Title = editTaskDto.Title,
                Description = editTaskDto.Description,
                Deadline = editTaskDto.Deadline != null ? editTaskDto.Deadline : DateTime.Now,
            });
            return CreatedAtAction(nameof(GetTaskById), new { id = editTaskDto.Title }, editTaskDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] Models.Entities.Task task)
        {
            if (task == null) return BadRequest("Task data is null");
            var filter = Builders<Models.Entities.Task>.Filter.Eq("Id", task.Id);
            var update = Builders<Models.Entities.Task>.Update
                .Set(u => u.Title, task.Title)
                .Set(u => u.Description, task.Description)
                .Set(u => u.Deadline, task.Deadline);
            var result = await _tasks.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0 ? Ok(task) : NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var filter = Builders<Models.Entities.Task>.Filter.Eq("Id", id);
            var result = await _tasks.DeleteOneAsync(filter);
            return result.DeletedCount > 0 ? Ok() : NotFound();
        }
    }
}
