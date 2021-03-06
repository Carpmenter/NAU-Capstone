﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAUReviewApplication.Models;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;

namespace NAUReviewApplication.Controllers
{
    public class AdminController : Controller
    {
        private NAUcountryContext context;
        private int SurveyID;
        private int QuestionID;
        public static List<Participant> selectedparts = new List<Participant>();

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

            var questions = getQuestionsBySurvey(SurveyID);

            ViewBag.questions = questions;

            return View(context.Question.ToList());
        }

        public ICollection<Question> getQuestionsBySurvey(int survID)
        {
            return context.SurveyQuestion.Include(q => q.Question)
                .Where(sq => sq.SurveyId == survID)
                .Select(sq => sq.Question).ToList();
        }

        public int getSurveyID()
        {
            int temp = 0;

            foreach (var item in context.Survey)
            {
                if (item.SurveyId > SurveyID)
                    temp = item.SurveyId;
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
            }

            return temp;
        }

        public IActionResult removeName(int surveyID)
        {
            int ID = surveyID;
            int count = selectedparts.Count();
            selectedparts.RemoveAt(count - 1);
            return RedirectToAction("emailPage", new { ID });
        }

        public IActionResult addName(int participant, int surveyID)
        {
            int ID = surveyID;
            foreach (Participant item in context.Participant)
            {
                if (item.ParticipantId == participant)
                {
                    selectedparts.Add(item);
                }
            }
            return RedirectToAction("emailPage", new { ID });
        }

        public IActionResult emailPage(int ID)
        {
            ViewBag.surveyID = ID;
            List<Participant> list = new List<Participant>();
            list = context.Participant.ToList();
            ViewBag.listofparts = list;
            ViewBag.set = selectedparts;
            return View();
        }

        public IActionResult SendEmail(string subject, string body, int surveyID)
        {
            string url;
            string userID = "";
            string email = "";
            int ID = surveyID;
            string servID = surveyID.ToString();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("murphy2009@hotmail.com"));

            if (selectedparts.Count == 0)
            {
                return RedirectToAction("emailPage", new { ID });
            }

            foreach (var item in selectedparts)
            {
                userID = item.ParticipantId.ToString();
                email = item.Username;

                if (message.To.Count > 0)
                {
                    message.To.RemoveAt(0);
                }

                message.To.Add(new MailboxAddress(email));

                url = "http://localhost:49404/User/UserPage?id=" + servID + "&part=" + userID;

                try
                {
                    message.Subject = subject;
                }
                catch (Exception ex)
                {
                    return RedirectToAction("emailPage", new { ID });
                }
                if (body == "" || body == null)
                {
                    return RedirectToAction("emailPage", new { ID });
                }

                message.Body = new TextPart("plain")
                {
                    Text = body + "\n" + url
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.live.com", 587, false);
                    client.Authenticate("murphy2009@hotmail.com", "2Bwealthy");
                    client.Send(message);
                    client.Disconnect(true);

                };
            }

            return RedirectToAction("Index", "Home");
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
            if(Category == 0)
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
        public IActionResult Create(string name, DateTime SurveyDate, string person, int type)
        {
            string holder;

            if(type == 0)
            {
                holder = null;
            }
            else
            {
                var temp = getPerson(person);

                if (temp == null)
                {
                    return RedirectToAction(nameof(SaveSurveyPage));
                }
                else
                {
                    holder = temp;
                }

            }

            var CreateSurvey = new Survey { Description = name, CreationDate = SurveyDate, Recipient = holder };

            if (ModelState.IsValid)
            {
                context.Add(CreateSurvey);
                context.SaveChanges();
                SurveyID = getSurveyID();
                return RedirectToAction(nameof(NewSurvey));
            }

            return View(CreateSurvey);
        }

        public string getPerson(string person)
        {
            var temp = context.Participant
                .Where(p => p.Username == person)
                .SingleOrDefault();

            if (temp == null)
                return null;
            else
                return temp.ToString();
        }
    }
}