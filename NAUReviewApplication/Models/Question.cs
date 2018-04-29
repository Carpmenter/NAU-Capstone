using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class Question
    {
        public Question()
        {
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public int QuestionId { get; set; }
        public int GroupId { get; set; }
        public int CategoryId { get; set; }
        public int Type { get; set; }
        public string Text { get; set; }
        public List<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();

        public Category Category { get; set; }
        public Group Group { get; set; }
        public ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
