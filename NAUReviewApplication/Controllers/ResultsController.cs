using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAUReviewApplication.Models;

namespace NAUReviewApplication.Controllers
{
    public class ResultsController : Controller
    {
        private NAUcountryContext context;

        public ResultsController(NAUcountryContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            //List of Completed Surveys
            // var completedSurveys = (from s in context.Surveys where s.Date > currentDate select s).ToList();
            return View(context.Survey.ToList());
        }

        public IActionResult Responses(string id)
        {
            var intData = Convert.ToInt32(id);

            if (id == null)
            {
                return NotFound();
            }

            var survey = from r in context.SurveyResponse where r.SurveyId.Equals(intData) select r;
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        public IActionResult Create()
        {
            DateTime sDate = new DateTime(2018, 10, 17);
            DateTime today = DateTime.Today;
            var testSurvey = new Survey { Description = "First test Survey Created", CreationDate = sDate };
            var testSurvey2 = new Survey { Description = "General Project Servey", CreationDate = today };
            var q1 = new Question { Type = 0, Text = "Rate overall experience" };
            var q2 = new Question { Type = 0, Text = "Rate how 4 C's were used" };
            var q3 = new Question { Type = 0, Text = "Effectively Mitigated Risks" };
            var sq1 = new SurveyQuestion();
            var sq2 = new SurveyQuestion();

            //Survey 1 mapping
            sq1.Survey = testSurvey;
            sq1.Question = q1;
            sq2.Survey = testSurvey;
            sq2.Question = q2;
            //Survey 2 mapping
            //sq2.Survey = testSurvey2;
            //sq2.Question = q1;
            //sq2.Question = q2;
            //sq2.Question = q3;
            //Linking data
            testSurvey.SurveyQuestions.Add(sq1);
            testSurvey.SurveyQuestions.Add(sq2);
            // add to context
            context.Survey.Add(testSurvey);
            context.Question.Add(q1);
            context.Question.Add(q2);

            //context.SurveyQuestion.Add(new SurveyQuestion { Survey = testSurvey, Question = q1 });
            //context.SurveyQuestion.Add(new SurveyQuestion { Survey = testSurvey, Question = q2 });
            //context.SurveyQuestion.Add(new SurveyQuestion { Survey = testSurvey2, Question = q1 });
            //context.SurveyQuestion.Add(new SurveyQuestion { Survey = testSurvey2, Question = q2 });
            // context.SurveyQuestion.Add(new SurveyQuestion { Survey = testSurvey2, Question = q3 });
            //context.SurveyQuestion.Add(new SurveyQuestion { Survey = s, Question = q });
            context.SaveChanges();

            return View();
        }
    }
}