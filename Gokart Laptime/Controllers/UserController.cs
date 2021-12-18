using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gokart_Laptime.Controllers
{
    public class UserController : Controller
    {
        private UserDAO userDAO;
        // GET: UserController
        public UserController(IConfiguration configuration)
        {
            userDAO = new UserDAO(configuration);
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegisterViewModel userRegistrationViewModel)
        {
            try
            {
                if (userDAO.RegisteredEmail(userRegistrationViewModel.Email)) ModelState.AddModelError("Email", "This email address is already in use!");

                if (userDAO.UsernameAlreadyInUse(userRegistrationViewModel.UserName)) ModelState.AddModelError("UserName", "This username is already in use! Please select another one!");

                if (!ModelState.IsValid) return View(userRegistrationViewModel);

                userDAO.RegisterUser(userRegistrationViewModel);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
