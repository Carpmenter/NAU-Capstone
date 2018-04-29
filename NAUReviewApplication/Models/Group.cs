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

        public int GroupId { get; set; }
        public string Name { get; set; }

        public ICollection<Participant> Participant { get; set; }
        public ICollection<Question> Question { get; set; }
    }
}
