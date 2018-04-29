using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class Participant
    {
        public Participant()
        {
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public int ParticipantId { get; set; }
        public int GroupId { get; set; }
        public string Username { get; set; }

        public Group Group { get; set; }
        public ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
