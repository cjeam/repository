using GameServerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameServerApi.Data

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<SpawnedObjects> SpawnedObjects { get; set; }
    }
}
