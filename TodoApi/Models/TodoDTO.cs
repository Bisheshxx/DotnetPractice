namespace TodoApi.Models
{
    public class TodoDTO
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public bool IsComplete { get; set; }

    }
}