using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class WebApplication3Context : DbContext
    {
        public WebApplication3Context (DbContextOptions<WebApplication3Context> options)
            : base(options)
        {
             Database.EnsureCreated();
             //Database.EnsureDeleted();
        }
        public static DbContextOptions<WebApplication3Context> GetSqlServerOptions()
        {
            // Загрузка конфігурації з appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Отримання рядка підключення з конфігурації
            var connectionString = configuration.GetConnectionString("WebApplication3Context");

            // Налаштування DbContextOptionsBuilder для використання SQL Server
            var options = new DbContextOptionsBuilder<WebApplication3Context>()
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }

        public DbSet<WebApplication3.Models.Coach> Coach { get; set; } = default!;

        public DbSet<WebApplication3.Models.Athlete>? Athlete { get; set; }

        public DbSet<WebApplication3.Models.Organization>? Organization { get; set; }

        public DbSet<WebApplication3.Models.Competition>? Competition { get; set; }

        public DbSet<WebApplication3.Models.Race>? Race { get; set; }

        public DbSet<WebApplication3.Models.RaceWithHandicap>? RaceWithHandicap { get; set; }

        public DbSet<WebApplication3.Models.Result>? Result { get; set; }

        public DbSet<WebApplication3.Models.FinishedRace>? FinishedRace { get; set; }

        public DbSet<WebApplication3.Models.FinishedRaceWithHandicap>? FinishedRaceWithHandicap { get; set; }

        public DbSet<WebApplication3.Models.Crew>? Crew { get; set; }

        public DbSet<WebApplication3.Models.CrewRower>? CrewRower { get; set; }

        public DbSet<WebApplication3.Models.CrewHandicap>? CrewHandicap { get; set; }

        public DbSet<WebApplication3.Models.CrewHandicapRower>? CrewHandicapRower { get; set; }

        public DbSet<WebApplication3.Models.ResultHandicap>? ResultHandicap { get; set; }
    }
}
