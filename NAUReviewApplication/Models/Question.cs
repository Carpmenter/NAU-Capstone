using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class Question
    {
        public Question()
        {
            SurveyQuestion = new HashSet<SurveyQuestion>();
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public int QuestionId { get; set; }
        public string GroupId { get; set; }
        public string CategoryId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }

        public Category Category { get; set; }
        public Group Group { get; set; }
        public ICollection<SurveyQuestion> SurveyQuestion { get; set; }
        public ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
