using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class SurveyQuestion
    {
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public int Id { get; set; }

        public Question Question { get; set; }
        public Survey Survey { get; set; }
    }
}
