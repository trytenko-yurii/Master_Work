using Master_Work.Models;
using Master_Work.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Master_Work.Entities.SQLUser;

namespace Master_Work.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly GenericRepository _gRepositoory;

        public HomeController(ILogger<HomeController> logger, GenericRepository genericRepository)
        {
            _logger = logger;
            _gRepositoory = genericRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(SQLRegister model)
        {
            if (ModelState.IsValid)
            {
                // Передача даних користувача в репозиторій
                _gRepositoory.AddUserAsync(
                    model.FirstName,         // Ім'я користувача
                    model.Email,        // Email
                    model.LastName,     // Прізвище
                    model.Password      // Пароль
                );

                // Після успішної реєстрації перенаправляємо користувача
                return RedirectToAction("Index");
            }

            // Якщо модель невалідна, повертаємо її на форму з помилками
            return View(model);
        }


        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
