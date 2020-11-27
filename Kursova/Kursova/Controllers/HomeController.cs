using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kursova.Models;
using Kursova.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Kursova.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        OutfitsContext dbContext;
        
        public HomeController(ILogger<HomeController> logger, OutfitsContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
            
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Authenticated") == "True")
            {
                ViewData["Authenticated"] = "True";
                ViewData["User"] = HttpContext.Session.GetString("User");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel logModel)
        {
            string userEmail = logModel.email;
            string userPassword = logModel.password;
            if(dbContext.Users.Any(u=>u.email == userEmail && u.password == userPassword))
            {
                
                HttpContext.Session.SetString("Authenticated", "True");
                ViewData["Authenticated"] = "True";
                HttpContext.Session.SetString("User", userEmail);
            }
            else
            {
                ViewData["UserData"] = "Wrong credentials";
            }
            return RedirectToAction("Shop","Shop");
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel LogModel = new LoginViewModel();

            return View(LogModel);
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel regModel)
        {
            User newUser = new User();

            newUser.email = regModel.email;
            newUser.password = regModel.password;
            newUser.FirstName = regModel.FirstName;
            newUser.LastName = regModel.LastName;
            if (dbContext.Users.Any(u=>u.email == newUser.email))
            {
                ViewData["UserData"] = "This user already exists";
            }
            else
            {
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
            }
          
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel regModel = new RegisterViewModel();
            return View(regModel);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
