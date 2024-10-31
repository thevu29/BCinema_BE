using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Users.Queries
{
    public class UserQuery : PaginationQuery
    {
        public string? Name { get; set; }
        public string? Email { get; set; }

        public UserQuery() : base()
        {
        }

        public UserQuery(string? name, string? email, int page, int size)
            : base(page, size)
        {
            Name = name;
            Email = email;
        }
    }
}
