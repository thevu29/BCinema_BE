using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Roles.Queries
{
    public class RoleQuery : PaginationQuery
    {
        public string? Search { get; set; }

        public RoleQuery() : base() {}

        public RoleQuery(int page, int size, string? search) : base(page, size)
        {
            Search = search;
        }
    }
}
