using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class Survey
    {
        public Survey()
        {
            Participant = new HashSet<Participant>();
            //SurveyQuestion = new HashSet<SurveyQuestion>();
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public List<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();

        public ICollection<Participant> Participant { get; set; }
        //public ICollection<SurveyQuestion> SurveyQuestion { get; set; }
        public ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
