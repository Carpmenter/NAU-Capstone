using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class Group
    {
        public Group()
        {
            Participant = new HashSet<Participant>();
            Question = new HashSet<Question>();
        }

        public string Description { get; set; }
        public string GroupId { get; set; }

        public ICollection<Participant> Participant { get; set; }
        public ICollection<Question> Question { get; set; }
    }
}
