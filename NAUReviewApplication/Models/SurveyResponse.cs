using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class SurveyResponse
    {
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public int ParticipantId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }

        public Participant Participant { get; set; }
        public Question Question { get; set; }
        public Survey Survey { get; set; }
    }
}
