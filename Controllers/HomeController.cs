using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ActivityCenter.Controllers
{
    public class HomeController : Controller
    {
        private ActivityCenterContext db;

        public HomeController(ActivityCenterContext context)
        {
            db = context;
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(user => user.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");

                    return View("Index");
                }

                string password = newUser.Password;
                bool IsLetter(char c)
                {
                    return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
                }

                bool IsDigit(char c)
                {
                    return c >= '0' && c <= '9';
                }

                bool IsSymbol(char c)
                {
                    return c > 32 && c < 127 && !IsDigit(c) && !IsLetter(c);
                }

                bool IsValidPassword(string password)
                {
                    return
                        password.Any(c => IsLetter(c)) &&
                        password.Any(c => IsDigit(c)) &&
                        password.Any(c => IsSymbol(c));
                }
                if (!IsValidPassword(password))
                {
                    ModelState.AddModelError("Password", "Password must contain at least one letter, one number, and one special character.");

                    return View("Index");
                }
                    

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                db.Add(newUser);
                db.SaveChanges();

                HttpContext.Session.SetInt32("userId", newUser.UserId);
                return RedirectToAction("Dashboard", "Plan");
            }

            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(user => user.Email == userSubmission.LoginEmail);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "Invalid email or password.");
                    return View("Index");
                }

                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, user.Password, userSubmission.LoginPassword);

                if (result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid email or password");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("userId", user.UserId);
                return RedirectToAction("Dashboard", "Plan");
            }

            return View("Login");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
