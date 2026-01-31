namespace TodoApi.Models
{
    public class CreateUserDTO
    {
        public required string First_Name { get; set; }
        public required string Last_Name { get; set; }
        public required int Age
        { get; set; }
        public string? Sex { get; set; }
        public string? Occupation { get; set; }
    }
}