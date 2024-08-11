using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        public static string result = "";
        [HttpPost, ActionName("CheckRegistration")]
        public string CheckRegistration()
        {
            try
            {
                // Перевіряємо, чи HttpContext не є null перед використанням
                if (HttpContext != null && HttpContext.Session.TryGetValue("AthleteId", out byte[] athleteIdBytes))
                {
                    // "AthleteId" існує, можна вважати, що користувач зареєстрований
                    int athleteId = BitConverter.ToInt32(athleteIdBytes, 0);
                    // Робимо щось з athleteId, наприклад, повертаємо персоналізовану інформацію
                    result = $"Користувач з id {athleteId} зареєстрований.";
                    // Робимо щось з athleteId, наприклад, повертаємо персоналізовану інформацію
                    return result;
                }
                // Робимо щось з athleteId, наприклад, повертаємо персоналізовану інформацію
                result = $"Користувач з не  зареєстрований.";
                // "AthleteId" не існує або HttpContext є null, користувач не зареєстрований

                return result;
            }
            catch (Exception ex)
            {    // Робимо щось з athleteId, наприклад, повертаємо персоналізовану інформацію
                result = $"Користувач з не зареєстрований.";
                // Логуємо помилку або робимо інші необхідні дії при виникненні помилки
                return result;
            }
        }
    }
}
