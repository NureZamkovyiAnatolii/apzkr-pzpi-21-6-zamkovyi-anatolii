using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class SignUp : Controller
    {
        private readonly WebApplication3Context _context;

        public SignUp(WebApplication3Context context)
        {
            _context = context;
        }
        public ActionResult SignUpPage()
        {
            ViewBag.role = "Тренер"; // Початкове значення ролі
            return View("SignUp");
        }
        public async Task<IActionResult> SignUpAll(
            string role, string name, string password)
        {
            ViewBag.Role = role; // Оновлення значення ролі при відправці форми
            var loginViewModel = new SignUpViewModel(); // Створити новий екземпляр моделі
            Console.WriteLine("role=" + ViewBag.Role);
            if (role == "Тренер")
            {
                // Якщо роль - "Тренер", перенаправте на дію "Create" контролера "Coach"
                return RedirectToAction("Create", "Coaches");
            }
            if (role == "Фанат")
            {
                // Якщо роль - "Тренер", перенаправте на дію "Create" контролера "Coach"
                return RedirectToAction("Create", "Fans");
            }
            if (role == "Спортсмен")
            {
                // Якщо роль - "Тренер", перенаправте на дію "Create" контролера "Coach"
                return RedirectToAction("Create", "Athletes");
            }
            if (role == "Організація")
            {
                // Якщо роль - "Тренер", перенаправте на дію "Create" контролера "Coach"
                return RedirectToAction("Create", "Organizations");
            }
            return View(loginViewModel);

        }

    }
}
