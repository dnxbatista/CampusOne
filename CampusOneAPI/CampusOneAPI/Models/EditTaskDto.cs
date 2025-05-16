namespace CampusOneAPI.Models
{
    public class EditTaskDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
