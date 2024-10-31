using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Roles.Queries
{
    public class RoleQuery : PaginationQuery
    {
        public string? Name { get; set; }

        public RoleQuery() : base()
        {
        }

        public RoleQuery(int page, int size, string name) : base(page, size)
        {
            Name = name;
        }
    }
}
