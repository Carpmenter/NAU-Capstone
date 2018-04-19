using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAUReviewApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace NAUReviewApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly NAUcountryContext context;
        private int SurveyID;

        public UserController(NAUcountryContext context)
        {
            this.context = context;
        }

        public IActionResult UserPage(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SurveyID = Convert.ToInt32(id);

            ViewBag.surveyID = SurveyID;

            // Get list of questions corresponding to SurveyID 
            var questions = getQuestionsBySurvey(SurveyID);

            if (questions == null)
            {
                return NotFound();
            }

            ViewBag.questions = questions;

            return View(context.Question.ToList());
        }

        public ICollection<Question> getQuestionsBySurvey(int survID)
        {
            return context.SurveyQuestion.Include(q => q.Question)
                .Where(sq => sq.SurveyId == survID)
                .Select(sq => sq.Question).ToList();
        }

        public IActionResult Save(int[] score, string[] comment)
        {
            int scores, question;
            var questions = getQuestionsBySurvey(1);// SurveyID);
            string comments;

            if (ModelState.IsValid)
            {
                for (int i = 0; i < score.Length; i++)
                {
                    scores = score[i];
                    question = questions.ElementAt(i).QuestionId;
                    comments = comment[i];

                    var surveyResponses = new SurveyResponse { SurveyId = 1, QuestionId = question, ParticipantId = 1, Score = scores, Comment = comments};

                    context.Add(surveyResponses);
                    context.SaveChanges();
                }
                return RedirectToAction(nameof(ThankYou));
            }
            return View();
        }

        public IActionResult ThankYou()
        {
            ViewData["Message"] = "Your ThankYou page.";

            return View();
        }

        public IActionResult Index()
        {
            return View(context.Question.ToList());
        }
    }
}