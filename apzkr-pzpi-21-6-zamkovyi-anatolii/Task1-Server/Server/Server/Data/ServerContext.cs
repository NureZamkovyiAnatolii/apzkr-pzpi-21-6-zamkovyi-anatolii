using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace Server.Data
{
    public interface IServerContext
    {
        DbSet<CrewHandicap> CrewHandicap { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        // Інші методи, які ви хочете включити
    }

    public class ServerContext : DbContext, IServerContext
    {
        public ServerContext (DbContextOptions<ServerContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication3.Models.Coach> Coach { get; set; } = default!;

        public DbSet<WebApplication3.Models.Athlete>? Athlete { get; set; }

        public DbSet<WebApplication3.Models.Organization>? Organization { get; set; }

        public DbSet<WebApplication3.Models.Competition>? Competition { get; set; }

        public DbSet<WebApplication3.Models.Crew>? Crew { get; set; }

        public DbSet<WebApplication3.Models.Race>? Race { get; set; }

        public DbSet<WebApplication3.Models.RaceWithHandicap>? RaceWithHandicap { get; set; }

        public virtual DbSet<WebApplication3.Models.CrewHandicap>? CrewHandicap { get; set; }

        public DbSet<WebApplication3.Models.Result>? Result { get; set; }

        public DbSet<WebApplication3.Models.ResultHandicap>? ResultHandicap { get; set; }

        public DbSet<WebApplication3.Models.CrewRower>? CrewRower { get; set; }

        public DbSet<WebApplication3.Models.FinishedRace>? FinishedRace { get; set; }

        public DbSet<WebApplication3.Models.FinishedRaceWithHandicap>? FinishedRaceWithHandicap { get; set; }
    }
}
