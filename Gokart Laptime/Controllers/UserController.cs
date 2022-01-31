using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;

namespace Gokart_Laptime.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserDAO userDAO;
        // GET: UserController
        public UserController(IUserDAO userDAO)
        {
            this.userDAO = userDAO;
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

        // GET: UserController/Register
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: UserController/Register
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

                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }


        // GET: UserController/Register
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (!string.IsNullOrEmpty(Request.QueryString.Value)) return RedirectToAction("Login");

            return View();
        }

        // GET: UserController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginAsync(UserLoginViewModel userLoginViewModel)
        {
            try
            {
                UserModel user = userDAO.GetUserByEmail(userLoginViewModel.Email);

                if (user is null || !BCrypt.Net.BCrypt.Verify(userLoginViewModel.Password, user.Password))
                {
                    ViewBag.Information = JsonConvert.SerializeObject(new { Type = "danger", Message = "Invalid user credentials!" });
                    return View();
                }

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(type: "RegisteredAt", user.RegisteredAt.ToString()),
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties { IsPersistent = true });
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "success", Message = "Successful login!" });

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            ViewBag.Information = JsonConvert.SerializeObject(new { Type = "info", Message = "Successful logout!" });
            return View(nameof(Login));
        }




        // GET: UserController/User/5
        [Authorize]
        public ActionResult UserProfile(int id)
        {
            if (id != Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value))
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "You are not allowed to do that!" });
                return RedirectToAction("Index", "Home");
            }

            UserModel user = userDAO.GetUserDetailsById(id);
            return View(user);
        }

        // GET 
        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            string userPassword = userDAO.GetUserDetailsById(Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value)).Password;

            if (!BCrypt.Net.BCrypt.Verify(changePasswordViewModel.CurrentPassword, userPassword))
            {
                ModelState.AddModelError("CurrentPassword", "The current password is not appropriate");
            }

            if (!ModelState.IsValid)
            {
                return View(changePasswordViewModel);
            }

            else
            {
                userDAO.UpdatePassword(Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value), changePasswordViewModel);
            }

            
            return View();
        }
    }
}
