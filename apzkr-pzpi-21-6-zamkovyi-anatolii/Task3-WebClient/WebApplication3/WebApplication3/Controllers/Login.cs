using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public LoginController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public IActionResult LoginPage()
        {
            return View("Login", new LoginViewModel());
        }

        public async Task<IActionResult> LoginAll(string role, string name, string password)
        {
            Console.WriteLine($"Attempting login for Role: {role}, Name: {name}");

            var loginRequest = new LoginRequest
            {
                Role = role,
                Name = name,
                Password = password
            };

            var client = _httpClientFactory.CreateClient();
            var requestContent = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                Console.WriteLine($"https://localhost:7237/api//Login");
                response = await client.PostAsync($"https://localhost:7237/api/Login", requestContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the server: {ex.Message}");
                var loginViewModel = new LoginViewModel { AlertMessage = "Error connecting to the server." };
                return View("Index", loginViewModel);
            }

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                Console.WriteLine($"Login response received: Success: {loginResponse.Success}, Message: {loginResponse.Message}");

                if (loginResponse.Success)
                {
                    if (role == "Тренер")
                    {
                        HttpContext.Session.SetInt32("CoachId", loginResponse.UserId.Value);
                        Console.WriteLine("Coach login successful. Redirecting to Coaches Index.");
                        return RedirectToAction("Index", "Coaches");
                    }
                    if (role == "Спортсмен")
                    {
                        HttpContext.Session.SetInt32("AthleteId", loginResponse.UserId.Value);
                        Console.WriteLine("Athlete login successful. Redirecting to Athletes Index.");
                        return RedirectToAction("Index", "Athletes");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("OrgId", loginResponse.UserId.Value);
                        Console.WriteLine("Organization login successful. Redirecting to Organizations Index.");
                        return RedirectToAction("Index", "Organizations");
                    }
                }
                else
                {
                    Console.WriteLine($"Login failed: {loginResponse.Message}");
                    var loginViewModel = new LoginViewModel { AlertMessage = loginResponse.Message };
                    return View("Index", loginViewModel);
                }
            }
            else
            {
                Console.WriteLine($"Error connecting to the server. Status code: {response.StatusCode}");
                var loginViewModel = new LoginViewModel { AlertMessage = "Error connecting to the server." };
                return View("Index", loginViewModel);
            }
        }

        public IActionResult Index()
        {
            Console.WriteLine("Navigating to Index page.");
            return View(new LoginViewModel());
        }
    }

    public class LoginRequest
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
    }

    public class ApiSettings
    {
        public string BaseApiUrl { get; set; }
    }

}
