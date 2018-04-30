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
            var survey = context.Survey.Single(s => s.SurveyId == surveyID);
            List<double> averages = new List<double>();
            List<SurveyResponse> recipientResponses = new List<SurveyResponse>();
            var groupScores = new List<Tuple<ICollection<Group>, List<double>>>();
            // Get list of questions corresponding to SurveyID 
            var questions = getQuestionsBySurvey(surveyID);

            foreach (var q in questions)
            {
                double temp;
                if (survey.Type == 1)
                {   // if individual survey extra data for self responses needed and average filtered
                    var recipient = getRecipient(surveyID);
                    var response = getResponseFor(q.QuestionId, surveyID, recipient.ParticipantId);
                    recipientResponses.Add(response);
                    temp = getAvgResponses(q.QuestionId, surveyID, recipient.ParticipantId);
                    averages.Add(temp);
                }
                else
                {
                    groupScores.Add(getAverageByGroup(q.QuestionId, surveyID));
                    temp = getAvgResponses(q.QuestionId, surveyID);
                    averages.Add(temp);
                }
            }

            if (survey.Type == 1)
            {
                ViewBag.Recipient = getRecipient(surveyID);
                ViewBag.RecipAnswers = recipientResponses;
            }

            ViewBag.GroupAvgs = groupScores;
            ViewBag.Comments = getComments(surveyID);
            ViewBag.Groups = getSurveyGroups(surveyID);
            ViewBag.Questions = getQuestionsBySurvey(surveyID);
            ViewBag.Survey = survey;
            ViewBag.Averages = averages;

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
            List<string> usernames = new List<string>();

            // Get all responses to questionID from surveyID
            var questionResponses = getQuestionResponses(intID, survID);
            var questions = context.Question.Single(q => q.QuestionId == intID);
            
            if (questionResponses == null)
            {
                return NotFound();
            }

            foreach(var u in questionResponses)
            {
                var temp = u.ParticipantId;
                usernames.Add((context.Participant.Single(p => p.ParticipantId == temp).Username));
            }

            ViewBag.Question = questions.Text;
            ViewBag.Usernames = usernames;

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
        
        public double getAvgResponses(int questID, int survID)
        {
            // Selects responses from question questID in survey survID
            // Then returns average for that response
            var avg = context.SurveyResponse.Where(sr =>
                 sr.QuestionId == questID &&
                 sr.SurveyId == survID).ToList();

            return Math.Round(avg.Select(x => x.Score).Average(), 2);
        }

        public double getAvgResponses(int questID, int survID, int recipientID)
        {
            // Selects responses from question questID in survey survID excluding recipient partID
            // Then returns average for that response
            var avg = context.SurveyResponse.Where(sr =>
                 sr.QuestionId == questID &&
                 sr.SurveyId == survID &&
                 sr.ParticipantId != recipientID).ToList();

            return Math.Round(avg.Select(x => x.Score).Average(), 2);
        }

        public ICollection<Group> getSurveyGroups(int survID)
        {
            // gets list of participants of a survey
            // then returns list of distinct groups participants are assigned to
            var participants = context.SurveyResponse.Where(sr => sr.SurveyId == survID)
                .Select(sr => sr.ParticipantId).Distinct().ToList();

            List<int> groupIDs = new List<int>();

            foreach (var p in participants)
            {
                var GID = context.Participant.Where(x => x.ParticipantId == p)
                    .Select(x => x.GroupId).Single();

                groupIDs.Add(GID);
            }

            groupIDs = groupIDs.Distinct().ToList();
            List<Group> groups = new List<Group>();

            foreach(var g in groupIDs)
            {
                groups.Add(context.Group.Single(x => x.GroupId == g));
            }

            return groups;
        }
        
        public Tuple<ICollection<Group>, List<double>> getAverageByGroup(int questID, int survID)
        {
            var responses = context.SurveyResponse.Where(sr =>
                 sr.QuestionId == questID &&
                 sr.SurveyId == survID).ToList();

            var groupScore = new List<Tuple<int, int>>();
            List<double> avgs = new List<double>();
            var groups = getSurveyGroups(survID);

            foreach(var r in responses)
            {
                var GID = context.Participant.Where(p => p.ParticipantId == r.ParticipantId)
                    .Select(p => p.GroupId).Single();
                int score = r.Score;
                groupScore.Add(Tuple.Create(GID, score));
            }

            for (int i=0; i< groups.Count; i++)
            {
                int count = 0;
                int score =0;
                int temp = groups.ElementAt(i).GroupId;
                foreach (var gs in groupScore)
                {
                    if (gs.Item1 == temp)
                    {
                        score += gs.Item2;
                        count++;
                    }
                }
                double avg = score / count;
                avgs.Add(Math.Round(avg, 2));
            }

            return Tuple.Create(groups, avgs);
        }

        public ICollection<SurveyResponse> getComments(int survID)
        {
            // returns all comments for a given survey
            List<SurveyResponse> comments = new List<SurveyResponse>();
            var responses = context.SurveyResponse.Where(sr => sr.SurveyId == survID).ToList();
            foreach (var r in responses)
            {
                if (r.Comment != null)
                {
                    comments.Add(r);
                }
            }

            return comments;
        }

        public Participant getRecipient(int survID)
        {
            //returns the participant object for whom is getting reviewed for a given survey
            var survey = context.Survey.Single(s => s.SurveyId == survID);
            var recipient = context.Participant.Single(p => p.Username == survey.Recipient);

            return recipient;
        }

        public SurveyResponse getResponseFor(int questID, int survID, int participantID)
        {
            var response = context.SurveyResponse.Where(sr =>
                sr.QuestionId == questID &&
                sr.SurveyId == survID &&
                sr.ParticipantId == participantID).Single();

            return response;
        }

    }
}