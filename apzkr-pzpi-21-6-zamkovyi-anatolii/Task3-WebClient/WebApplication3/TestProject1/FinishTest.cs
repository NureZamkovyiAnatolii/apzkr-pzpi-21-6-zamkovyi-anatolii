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
    public class FinishTest
    {
        [Fact]
        public async Task Create_ValidModel_ReturnsRedirectToActionResult()
        {
            var options = WebApplication3Context.GetSqlServerOptions();

            using (var context = new WebApplication3Context(options))
            {
                var controller = new FinishedRacesController(context);
                var finishedRace = new FinishedRace
                {
                    RaceId = 1,
                    FinishTime = DateTime.Now
                };

                var result = await controller.Create(finishedRace);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);

                var createdRace = await context.FinishedRace.FindAsync(finishedRace.FinishedRaceId);
                Assert.NotNull(createdRace);
                Assert.Equal(finishedRace.RaceId, createdRace.RaceId);
                Assert.Equal(finishedRace.FinishTime, createdRace.FinishTime);
            }
        }
        [Fact]
        public async Task Edit_ValidId_ReturnsViewWithFinishedRace()
        {
            var options = WebApplication3Context.GetSqlServerOptions();

            using (var context = new WebApplication3Context(options))
            {
                var finishedRace = new FinishedRace
                {
                    RaceId = 1,
                    FinishTime = DateTime.Now
                };
                context.FinishedRace.Add(finishedRace);
                await context.SaveChangesAsync();
            }

            using (var context = new WebApplication3Context(options))
            {
                var controller = new FinishedRacesController(context);
                var result = await controller.Edit(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<FinishedRace>(viewResult.Model);
                Assert.Equal(1, model.FinishedRaceId);
            }
        }

        [Fact]
        public async Task Edit_InvalidId_ReturnsNotFound()
        {
            var options = WebApplication3Context.GetSqlServerOptions();
            using (var context = new WebApplication3Context(options))
            {
                var controller = new FinishedRacesController(context);
                var result = await controller.Edit(99); // Невірний ID, якого не існує в базі даних

                Assert.IsType<NotFoundResult>(result);
            }
        }
    }

}

