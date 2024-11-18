namespace BCinema.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Avatar { get; set; }
        public int? Point { get; set; } = 0;
        public bool IsActivated { get; set; }
        public string? Provider { get; set; }
        public string Role { get; set; } = default!;
    }
}
