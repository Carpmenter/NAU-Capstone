﻿using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class Survey
    {
        public Survey()
        {
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public int SurveyId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int Type { get; set; }
        public string Recipient { get; set; }
        public List<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();

        public ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
