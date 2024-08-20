using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService
{
    public class UserServiceDbContext : DbContext
    {
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
