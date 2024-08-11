using Xunit;
using WebApplication3.Controllers;
using WebApplication3.Models;
using WebApplication3.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace TestProject1
{
    public class UserTests
    {
       
    
        [Fact]
        public async Task Create_ValidAthlete_RedirectsToIndex()
        {
            var options = WebApplication3Context.GetSqlServerOptions();

            using (var context = new WebApplication3Context(options))
            {
                context.Database.EnsureCreated(); // Створення бази даних
                context.Coach.Add(
                    new Coach {  CoachName = "Coach 1",
                    BirthDate = DateTime.Now.AddYears(-60),
                    Country = "Ukraine",
                    Password = "123",
                    PhoneNumber =12234
                    });
                context.SaveChanges();
            }

            using (var context = new WebApplication3Context(options))
            {
                var controller = new AthletesController(context);
                var athlete = new Athlete
                {
                    AthleteName = "Athlete 1",
                    Password = "password",
                    BirthDate = DateTime.Now.AddYears(-20),
                    PhoneNumber = 123456789,
                    CoachId = 1
                };
                var result = await controller.Create(athlete);

                Console.WriteLine(result);
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Null(viewResult.ViewName); // Якщо ViewName не заданий явно, він буде null
                Assert.True(context.Athlete.Any(a => a.AthleteId == athlete.AthleteId));
            }
        }

        [Fact]
        public async Task Create_InvalidCoachId_ReturnsViewWithModelError()
        {
            var options = WebApplication3Context.GetSqlServerOptions();

            using (var context = new WebApplication3Context(options))
            {
                var controller = new AthletesController(context);
                var athlete = new Athlete
                {
                    AthleteName = "Athlete 1",
                    Password = "password",
                    BirthDate = DateTime.Now.AddYears(-20),
                    PhoneNumber = 123456789,
                    CoachId = 99 // Несуществующий ID тренера
                };

                var result = await controller.Create(athlete);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.False(controller.ModelState.IsValid);
                Assert.True(controller.ModelState.ContainsKey("CoachId"));
                Assert.Equal("Такого тренера не існує.", controller.ModelState["CoachId"].Errors.First().ErrorMessage);
            }
        }
    }
}