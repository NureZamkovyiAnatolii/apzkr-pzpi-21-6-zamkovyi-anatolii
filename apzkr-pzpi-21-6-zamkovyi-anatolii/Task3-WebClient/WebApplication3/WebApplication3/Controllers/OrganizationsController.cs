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
    public class OrganizationsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public OrganizationsController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        // GET: Organizations
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Organizations");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var organizations = JsonConvert.DeserializeObject<IEnumerable<Organization>>(jsonData);
                return View(organizations);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Organizations/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var organization = JsonConvert.DeserializeObject<Organization>(jsonData);
                if (organization == null)
                {
                    return NotFound();
                }
                return View(organization);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationId,OrganizationName,Password,PhoneNumber,Country")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                organization.Password = BC.HashPassword(organization.Password);

                var content = new StringContent(JsonConvert.SerializeObject(organization), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_apiSettings.BaseApiUrl}/Organizations", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Error creating the organization.");
                }
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Organizations/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var organization = JsonConvert.DeserializeObject<Organization>(jsonData);
                if (organization == null)
                {
                    return NotFound();
                }
                return View(organization);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrganizationId,OrganizationName,Password,PhoneNumber,Country")] Organization organization)
        {
            if (id != organization.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(organization), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{_apiSettings.BaseApiUrl}/Organizations/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Problem("Error updating the organization.");
                }
            }
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiSettings.BaseApiUrl}/Organizations/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var organization = JsonConvert.DeserializeObject<Organization>(jsonData);
                if (organization == null)
                {
                    return NotFound();
                }
                return View(organization);
            }
            else
            {
                return Problem("Error fetching data from the API.");
            }
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiSettings.BaseApiUrl}/Organizations/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Problem("Error deleting the organization.");
            }
        }

        private bool OrganizationExists(int id)
        {
            // This method checks if the organization exists in the database
            // Since we are working with API, this method might not be necessary.
            return true;
        }
    }
}
