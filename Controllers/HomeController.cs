using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltLogin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BeltLogin.Controllers
{
    public class HomeController : Controller
    {
        private BeltContext
         _context;

        public HomeController(BeltContext
         context){
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            System.Console.WriteLine("Inside Login");
            return View("Login");
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser(string Email, string Password)
        {
            if(ModelState.IsValid)
            {
                User loginu = _context.Users.SingleOrDefault(a => a.Email == Email);
                // if (loginu != null)
                // {
                    string passMatch = loginu.Password;
                    string EmailMatch = loginu.Email;
                    var Hasher = new PasswordHasher<User>();
                    // Pass the user object, the hashed Password, and the PasswordToCheck
                    if (Password != null)
                    {
                        if(0 != Hasher.VerifyHashedPassword(loginu, loginu.Password, Password))
                        {
                            var param = loginu.UserId;
                            HttpContext.Session.SetInt32("user_id", param);
                            HttpContext.Session.SetInt32("logged", 1);
                            return RedirectToAction("Index", "Activitie");
                        }else{
                            ModelState.AddModelError("Password", "Password does not match.");
                            return View("Login");
                        }
                    }else{
                        ModelState.AddModelError("Password", "Please enter a Password.");
                        return View("Login");
                    }
                // }else{
                //     ModelState.AddModelError("Email", "You dont have an account. Please register.");
                //     return View("Login");
                // }
            }
            return View("Login");
        }


        [Route("register")]
        public IActionResult RegisterUser(ValidRegUser user)
        {
            if(ModelState.IsValid)
            {
                // add if check for already in DB
                User EmailMatch = _context.Users.SingleOrDefault(a => a.Email == user.Email);
                if (EmailMatch == null)
                {
                    BuildUser(user);
                    User Emailpull = _context.Users.SingleOrDefault(a => a.Email == user.Email);
                    var id = Emailpull.UserId;
                    HttpContext.Session.SetInt32("user_id", id);
                    HttpContext.Session.SetInt32("logged", 1);
                    return RedirectToAction("Index", "Activitie");
                }
                else
                {
                    ModelState.AddModelError("Email", "Email is already associated to an account.");
                    return View("Index");
                }

            }
            return View("Index");
        }

        private void BuildUser(ValidRegUser user)
        {

            User NewUser = new User{
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            string hashed = Hasher.HashPassword(NewUser, user.Password);
            NewUser.Password = hashed;
            _context.Users.Add(NewUser);
            _context.SaveChanges();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
