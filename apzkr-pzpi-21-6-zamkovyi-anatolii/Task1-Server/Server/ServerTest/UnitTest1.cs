using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Controllers;
using Server.Data;
using WebApplication3.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using WebApplication3.Data;

namespace ServerTest
{
    [TestClass]
    public class CrewHandicapsControllerTests
    {
        public static DbContextOptions<ServerContext> GetSqlServerOptions()
        {
            // Загрузка конфігурації з appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Отримання рядка підключення з конфігурації
            var connectionString = configuration.GetConnectionString("ServerContext");

            // Налаштування DbContextOptionsBuilder для використання SQL Server
            var options = new DbContextOptionsBuilder<ServerContext>()
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }
        [Fact]
        public async Task AssignRandomStartNumbersAsync_OnlyUpdatesSpecificRaceWithHandicapId()
        {
            var options = GetSqlServerOptions();

            // Створення тестових даних
            using (var context = new ServerContext(options))
            {
                // Створення 10 екземплярів з RaceWithHandicapId = 1
                for (int i = 1; i <= 10; i++)
                {
                    context.CrewHandicap.Add(new CrewHandicap
                    {
                        BoatType = BoatType.Single,
                        CompetitionId = 1,
                        RaceWithHandicapId = 1,
                        StartNumber = 0 // Встановлення початкового значення для StartNumber
                    });
                }

                // Створення 2 екземплярів з RaceWithHandicapId = 2
                for (int i = 11; i <= 12; i++)
                {
                    context.CrewHandicap.Add(new CrewHandicap
                    {
                        BoatType = BoatType.Single,
                        CompetitionId = 1,
                        RaceWithHandicapId = 2,
                        StartNumber = 0 // Встановлення початкового значення для StartNumber
                    });
                }

                await context.SaveChangesAsync();
            }

            // Перевірка функціональності
            using (var context = new ServerContext(options))
            {
                var controller = new CrewHandicapsController(context);
                var result = await controller.AssignRandomStartNumbers(1);

                // Перевірка, що стартові номери були оновлені для CrewHandicaps з RaceWithHandicapId = 1
                var updatedCrewHandicaps = context.CrewHandicap.Where(ch => ch.RaceWithHandicapId == 1).ToList();
                Xunit.Assert.All(updatedCrewHandicaps, ch => Xunit.Assert.NotEqual(0, ch.StartNumber));

                // Перевірка, що стартові номери не були змінені для CrewHandicaps з RaceWithHandicapId = 2
                var untouchedCrewHandicaps = context.CrewHandicap.Where(ch => ch.RaceWithHandicapId == 2).ToList();
                Xunit.Assert.All(untouchedCrewHandicaps, ch => Xunit.Assert.Equal(0, ch.StartNumber));
            }
        }
    }
}
