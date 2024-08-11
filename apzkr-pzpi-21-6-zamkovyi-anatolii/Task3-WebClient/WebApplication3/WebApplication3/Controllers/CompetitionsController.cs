using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public CompetitionsController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        // GET: Competitions
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Competitions");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var coaches = JsonConvert.DeserializeObject<IEnumerable<Competition>>(jsonData);
                return View(coaches);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // GET: Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/api/Competitions/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var competition = JsonConvert.DeserializeObject<Competition>(jsonData);
                    if (competition == null)
                    {
                        return NotFound();
                    }
                    return View(competition);
                }
                else
                {
                    return Problem("Error fetching data from the API.");
                }
            }
            catch (Exception ex)
            {
                return Problem($"Exception encountered: {ex.Message}");
            }
        }

        // GET: Competitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Competitions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompetitionId,OrganizationId,CoachId,CompetitionName,CompetitionCountry,CompetitionCity,RegistrationDeadline,CompetitioneDate")] Competition competition)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = _httpClientFactory.CreateClient();
                    var content = new StringContent(JsonConvert.SerializeObject(competition), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{_apiSettings.BaseApiUrl}/api/Competitions", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return Problem("Error creating the competition.");
                    }
                }
                return View(competition);
            }
            catch (Exception ex)
            {
                return Problem($"Exception encountered: {ex.Message}");
            }
        }

        // GET: Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/api/Competitions/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var competition = JsonConvert.DeserializeObject<Competition>(jsonData);
                    if (competition == null)
                    {
                        return NotFound();
                    }
                    return View(competition);
                }
                else
                {
                    return Problem("Error fetching data from the API.");
                }
            }
            catch (Exception ex)
            {
                return Problem($"Exception encountered: {ex.Message}");
            }
        }

        // POST: Competitions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("CompetitionId,OrganizationId,CoachId,CompetitionName,CompetitionCountry,CompetitionCity,RegistrationDeadline,CompetitioneDate")] Competition competition)
        {
            try
            {
                if (id != competition.CompetitionId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var client = _httpClientFactory.CreateClient();
                    var content = new StringContent(JsonConvert.SerializeObject(competition), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PutAsync($"{_apiSettings.BaseApiUrl}/api/Competitions/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return Problem("Error updating the competition.");
                    }
                }
                return View(competition);
            }
            catch (Exception ex)
            {
                return Problem($"Exception encountered: {ex.Message}");
            }
        }

        // GET: Competitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/api/Competitions/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var competition = JsonConvert.DeserializeObject<Competition>(jsonData);
                    if (competition == null)
                    {
                        return NotFound();
                    }
                    return View(competition);
                }
                else
                {
                    return Problem("Error fetching data from the API.");
                }
            }
            catch (Exception ex)
            {
                return Problem($"Exception encountered: {ex.Message}");
            }
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"{_apiSettings.BaseApiUrl}/api/Competitions/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Error deleting the competition.");
                }
            }
            catch (Exception ex)
            {
                return Problem($"Exception encountered: {ex.Message}");
            }
        }

    }
}
