using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Controllers;
using WebApplication3.Data;
using WebApplication3.Models;

namespace TestProject1
{
    public class CrewTests
    {
        [Fact]
        public async Task Create_CrewRower_ValidModel_RedirectsToIndex()
        {
            var options = WebApplication3Context.GetSqlServerOptions();

            using (var context = new WebApplication3Context(options))
            {
                var controller = new CrewRowersController(context);
                var crewRower = new CrewRower
                {
                    AthleteId = 1,
                    CrewId = 1
                };

                var result = await controller.Create(crewRower);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.True(context.CrewRower.Any(cr => cr.CrewRowerId == crewRower.CrewRowerId));
            }
        }

        [Fact]
        public async Task Create_Crew_ValidModel_RedirectsToIndex()
        {
            var options = WebApplication3Context.GetSqlServerOptions();

            using (var context = new WebApplication3Context(options))
            {
                var controller = new CrewsController(context);
                var crew = new Crew
                {
                    BoatType = BoatType.Single,
                    CompetitionId = 1,
                    RaceId = 1
                };

                var result = await controller.Create(crew);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.True(context.Crew.Any(c => c.CrewId == crew.CrewId));
            }
        }

        [Fact]
        public async Task Create_Race_ValidModel_RedirectsToIndex()
        {
            var options = WebApplication3Context.GetSqlServerOptions();

            using (var context = new WebApplication3Context(options))
            {
                var controller = new RacesController(context);
                var race = new Race
                {
                    StartTime = DateTime.Now,
                    CompetitionId = 1,
                    Stage = "Final"
                };

                var result = await controller.Create(race);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.True(context.Race.Any(r => r.RaceId == race.RaceId));
            }
        }
    }
}
