using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebApplication3.Data;
using WebApplication3.Models;
using BC = BCrypt.Net.BCrypt;

namespace WebApplication3.Controllers
{
    public class CoachesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public CoachesController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        // GET: Coaches
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Coaches");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var coaches = JsonConvert.DeserializeObject<IEnumerable<Coach>>(jsonData);
                return View(coaches);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // GET: Coaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Coaches/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var coach = JsonConvert.DeserializeObject<Coach>(jsonData);
                if (coach == null)
                {
                    return NotFound();
                }
                return View(coach);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // GET: Coaches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoachId,CoachName,Password,BirthDate,PhoneNumber,Country")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                coach.Password = BC.HashPassword(coach.Password);

                var content = new StringContent(JsonConvert.SerializeObject(coach), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_apiSettings.BaseApiUrl}/Coaches", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Error creating the coach.");
                }
            }
            return View(coach);
        }

        // GET: Coaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Coaches/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var coach = JsonConvert.DeserializeObject<Coach>(jsonData);
                if (coach == null)
                {
                    return NotFound();
                }
                return View(coach);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // POST: Coaches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CoachId,CoachName,Password,BirthDate,PhoneNumber,Country,CoachSport")] Coach coach)
        {
            if (id != coach.CoachId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(coach), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{_apiSettings.BaseApiUrl}/Coaches/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Error updating the coach.");
                }
            }
            return View(coach);
        }

        // GET: Coaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Coaches/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var coach = JsonConvert.DeserializeObject<Coach>(jsonData);
                if (coach == null)
                {
                    return NotFound();
                }
                return View(coach);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiSettings.BaseApiUrl}/Coaches/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Problem("Error deleting the coach.");
            }
        }
    }
}
