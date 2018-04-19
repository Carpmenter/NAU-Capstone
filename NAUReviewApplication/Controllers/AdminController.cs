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
        private int SurveyID;
        private int QuestionID;

        public AdminController(NAUcountryContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Survey.ToList());
        }

        public IActionResult SaveSurveyPage()
        {
            return View();
        }

        public IActionResult NewSurvey()
        {
            ViewData["Message"] = "Your NewSurvey page.";

            SurveyID = getSurveyID();

            //List<int> listOFQuestions = new List<int>();

            var questions = getQuestionsBySurvey(SurveyID);

            ViewBag.questions = questions;

            /*foreach (var q in questions)
            {
                int temp = getQuestions(q.QuestionId, SurveyID);
                listOFQuestions.Add(temp);
            }*/

            return View(context.Question.ToList());
        }

        public ICollection<Question> getQuestionsBySurvey(int survID)
        {
            return context.SurveyQuestion.Include(q => q.Question)
                .Where(sq => sq.SurveyId == survID)
                .Select(sq => sq.Question).ToList();
        }

       /* public int getQuestions(int questID, int survID)
        {
            // Selects responses from question questID in survey survID
            // Then returns average for that response
            var que = context.SurveyResponse.Where(sr =>
                 sr.QuestionId == questID &&
                 sr.SurveyId == survID).ToList();

            return avg.Select(x => x.Score).Average();
        }*/

        public int getSurveyID()
        {
            int temp = 0;

            foreach (var item in context.Survey)
            {
                if (item.SurveyId > SurveyID)
                    temp = item.SurveyId;
                //context.Survey.Where(sp => sp.SurveyId > SurveyID).Select(p => p.SurveyId);
            }

            return temp;
        }

        public int getQuestionID()
        {
            int temp = 0;

            foreach (var item in context.Question)
            {
                if (item.QuestionId > QuestionID)
                    temp = item.QuestionId;
                //context.Survey.Where(sp => sp.SurveyId > SurveyID).Select(p => p.SurveyId);
            }

            return temp;
        }

        /* public async Task<IActionResult> Select(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var allSurveys = await context.Question
                 .SingleOrDefaultAsync(m => m.SurveyQuestion.Survey.SurveyId == id);
             if (allSurveys == null)
             {
                 return NotFound();
             }

             return View(allSurveys);
         }
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddQuestion (string Question, int F_N, int Category)
        {
            if(Question == null)
            {
                return RedirectToAction(nameof(NewSurvey));
            }
            var CreateQuestion = new Question { GroupId = 1, CategoryId = Category, Text = Question, Type = F_N };
            SurveyID = getSurveyID();

            if (ModelState.IsValid)
            {
                context.Add(CreateQuestion);
                context.SaveChanges();

                QuestionID = getQuestionID();
                var CreateLink = new SurveyQuestion { QuestionId = QuestionID, SurveyId = SurveyID };

                context.Add(CreateLink);
                context.SaveChanges();

                return RedirectToAction(nameof(NewSurvey));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string name, DateTime SurveyDate)
        {
            var CreateSurvey = new Survey { Description = name, CreationDate = SurveyDate };

            if (ModelState.IsValid)
            {
                context.Add(CreateSurvey);
                context.SaveChanges();
                SurveyID = getSurveyID();
                return RedirectToAction(nameof(NewSurvey));
            }

            return View(CreateSurvey);
        }
    }
}