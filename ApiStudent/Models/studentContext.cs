using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiStudent.Models
{
    public class studentContext : IdentityDbContext<UserEntity,UserRole,int>
    {

        public studentContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAll> StudentsAll { get; set; }
    }
}
