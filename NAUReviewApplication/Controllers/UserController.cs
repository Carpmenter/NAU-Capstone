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
        private int ParticipantID;

        public UserController(NAUcountryContext context)
        {
            this.context = context;
        }

        public IActionResult UserPage(string id, string part)
        {
            if (id == null)
            {
                return NotFound();
            }

            SurveyID = Convert.ToInt32(id);
            ParticipantID = Convert.ToInt32(part);

            ViewBag.surveyID = SurveyID;
            ViewBag.participantID = ParticipantID;

            // Get list of questions corresponding to SurveyID 
            var questions = getQuestionsBySurvey(SurveyID);

            if (questions == null)
            {
                return NotFound();
            }

            ViewBag.questions = questions;

            return View();
        }

        public ICollection<Question> getQuestionsBySurvey(int survID)
        {
            return context.SurveyQuestion.Include(q => q.Question)
                .Where(sq => sq.SurveyId == survID)
                .Select(sq => sq.Question).ToList();
        }

        [HttpPost]
        public IActionResult Save(int[] score, string[] comment, int SurveyID, int ParticipantID)
        {
            int scores,question,scount = 0, ccount = 0; 
            var questions = getQuestionsBySurvey(SurveyID);
            string comments;

            if (ModelState.IsValid)
            {
                for (int i = 0; i < (score.Length + comment.Length); i++)
                {
                    question = questions.ElementAt(i).QuestionId;

                    if(questions.ElementAt(i).Type.ToString().Equals("0"))
                    {
                        comments = comment[ccount];
                        scores = -1;
                        ccount++;
                    }
                    else
                    {
                        comments = null;
                        scores = score[scount];
                        scount++;
                    }

                    var surveyResponses = new SurveyResponse { SurveyId = SurveyID, QuestionId = question, ParticipantId = ParticipantID, Score = scores, Comment = comments};

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