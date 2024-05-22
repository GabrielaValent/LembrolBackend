using Microsoft.EntityFrameworkCore;
using backend_lembrol.Entity;

namespace backend_lembrol.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<DaysOfWeek> DaysOfWeek => Set<DaysOfWeek>();
        public DbSet<SpecificDates> SpecificDates => Set<SpecificDates>();
    }
}