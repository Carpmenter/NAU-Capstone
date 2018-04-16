using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAUReviewApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace NAUReviewApplication.Controllers
{
    public class ResultsController : Controller
    {
        private NAUcountryContext context;
        private int surveyID;

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
            if (id == null)
            {
                return NotFound();
            }

            surveyID = Convert.ToInt32(id);

            // Get list of questions corresponding to SurveyID 
            var questions = getQuestionsBySurvey(surveyID);

            if (questions == null)
            {
                return NotFound();
            }

            return View(Tuple.Create(questions, surveyID));
        }

        public IActionResult QuestionResults(string id, int survID)
        {
            if (id == null)
            {
                return NotFound();
            }

            int intID = Convert.ToInt32(id);

            // Get all responses to questionID from surveyID
            var questionResponses = getQuestionResponses(intID, survID);

            if (questionResponses == null)
            {
                return NotFound();
            }
            
            return View(questionResponses);
        }

        public ICollection<Question> getQuestionsBySurvey(int survID)
        {
            return context.SurveyQuestion.Include(q => q.Question)
                .Where(sq => sq.SurveyId == survID)
                .Select(sq => sq.Question).ToList();
        }

        public ICollection<SurveyResponse> getQuestionResponses(int questID, int survID)
        {
            // Select all responses from question questID in survey survID
            return context.SurveyResponse.Where(sr =>
                 sr.QuestionId == questID &&
                 sr.SurveyId == survID)
                 .ToList();
        }


        public IActionResult Create()
        {
            DateTime sDate = new DateTime(2018, 10, 17);
            DateTime today = DateTime.Today;
            var testSurvey = new Survey { Description = "First test Survey Created", CreationDate = sDate };
            var testSurvey2 = new Survey { Description = "General Project Servey", CreationDate = today };
            var q1 = new Question { Type = 0, Text = "Effectively Mitigated Risks" };
            var q2 = new Question { Type = 1, Text = "List one thing you can improve on" };
            var q3 = new Question { Type = 0, Text = "Effectively Mitigated Risks" };
            var sq1 = new SurveyQuestion();
            var sq2 = new SurveyQuestion();

            /*** General Process for adding entities with many-to-many relationships ***/
            /*
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
            //context.Survey.Add(testSurvey);
            context.Question.Add(q1);
            context.Question.Add(q2);
            */
            /*** Included when adding large number of entities is needed ***
             
            public async Task SaveEntities(IEnumerable<SurveyQuestion> surveyQuestions)
            {
                int i = 0;
                foreach (var sq in surveyQuestions)
                {
                    context.Set<Survey>().Add(SurveyQuestion.Survey);
                    context.Set<Question>().Add(SurveyQuestion.Question);
                    context.Set<SurveyQuestion>().Add(sq);
                    i++;
                    if (i == 99)
                    {
                        await context.SaveChangesAsync();
                        i = 0;
                    }
                }
                await context.SaveChangesAsync();
            } *************************************************************/

            context.SaveChanges();

            return View();
        }

    }
}