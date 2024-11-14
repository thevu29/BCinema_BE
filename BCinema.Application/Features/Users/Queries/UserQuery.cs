using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Users.Queries
{
    public class UserQuery : PaginationQuery
    {
        public string? Search { get; set; }
        public Guid? Role { get; set; }

        public UserQuery() : base() {}

        public UserQuery(string? search, Guid? role, int page, int size) : base(page, size)
        {
            Search = search;
            Role = role;
        }
    }
}
