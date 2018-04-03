using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAUReviewApplication.Models;

namespace NAUReviewApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly NAUcountryContext _context;

        public IActionResult Index(string usernm, string passwrd)
        {
            string username = usernm;
            string password = passwrd;

            foreach (Admin a in _context.Admin)
            {
                if (a.Username == username && a.Password == password)
                {
                    
                    return View();
                }
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            

            return View();
        }
    }
}
