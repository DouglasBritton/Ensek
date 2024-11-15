using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(SeedAccounts());
        }

        private static Account[] SeedAccounts()
        {
            return
            [
                new() { Id = 2344, FirstName = "Tommy", SurnameName = "Test" },
                new() { Id = 2233, FirstName = "Barry", SurnameName = "Test" },
                new() { Id = 8766, FirstName = "Sally", SurnameName = "Test" },
                new() { Id = 2345, FirstName = "Jerry", SurnameName = "Test" },
                new() { Id = 2346, FirstName = "Ollie", SurnameName = "Test" },
                new() { Id = 2347, FirstName = "Tara", SurnameName = "Test" },
                new() { Id = 2348, FirstName = "Tammy", SurnameName = "Test" },
                new() { Id = 2349, FirstName = "Simon", SurnameName = "Test" },
                new() { Id = 2350, FirstName = "Colin", SurnameName = "Test" },
                new() { Id = 2351, FirstName = "Gladys", SurnameName = "Test" },
                new() { Id = 2352, FirstName = "Greg", SurnameName = "Test" },
                new() { Id = 2353, FirstName = "Tony", SurnameName = "Test" },
                new() { Id = 2355, FirstName = "Arthur", SurnameName = "Test" },
                new() { Id = 2356, FirstName = "Craig", SurnameName = "Test" },
                new() { Id = 6776, FirstName = "Laura", SurnameName = "Test" },
                new() { Id = 4534, FirstName = "JOSH", SurnameName = "TEST" },
                new() { Id = 1234, FirstName = "Freya", SurnameName = "Test" },
                new() { Id = 1239, FirstName = "Noddy", SurnameName = "Test" },
                new() { Id = 1240, FirstName = "Archie", SurnameName = "Test" },
                new() { Id = 1241, FirstName = "Lara", SurnameName = "Test" },
                new() { Id = 1242, FirstName = "Tim", SurnameName = "Test" },
                new() { Id = 1243, FirstName = "Graham", SurnameName = "Test" },
                new() { Id = 1244, FirstName = "Tony", SurnameName = "Test" },
                new() { Id = 1245, FirstName = "Neville", SurnameName = "Test" },
                new() { Id = 1246, FirstName = "Jo", SurnameName = "Test" },
                new() { Id = 1247, FirstName = "Jim", SurnameName = "Test" },
                new() { Id = 1248, FirstName = "Pam", SurnameName = "Test" }
            ];
        }
    }
}
