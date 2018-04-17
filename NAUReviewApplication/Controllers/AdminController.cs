using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAUReviewApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace NAUReviewApplication.Controllers
{
    public class AdminController : Controller
    {
        private NAUcountryContext context;

        public AdminController(NAUcountryContext context)
        {
            this.context = context;
        }

        public IActionResult Survey()
        {
            return View(context.Question.ToList());
        }

        public IActionResult NewSurvey()
        {
            ViewData["Message"] = "Your NewSurvey page.";

            return View(context.Survey.ToList());
        }

        public async Task<IActionResult> Select(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allSurveys = await context.Question
                .SingleOrDefaultAsync(m => m.SurveyQuestion.Survey.ID == id);
            if (allSurveys == null)
            {
                return NotFound();
            }

            return View(allSurveys);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion (string Question, int F_N, int Category)
        {
            if (ModelState.IsValid)
            {
                //context.Add(Surveys);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(NewSurvey));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(string name, int I_G, DateTime SurveyDate)
        {
            if (ModelState.IsValid)
            {
                //context.Add(AllSurveys);
                context.SaveChanges();
                return RedirectToAction(nameof(Survey));
            }
            return RedirectToAction(nameof(Survey));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}