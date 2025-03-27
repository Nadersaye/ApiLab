using Microsoft.EntityFrameworkCore;

namespace LabApi.Models
{
    public class ITIDbContext : DbContext
    {
        public ITIDbContext() { }
        public ITIDbContext(DbContextOptions<ITIDbContext> options) : base(options) { }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
