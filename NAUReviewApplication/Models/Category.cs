using System;
using System.Collections.Generic;

namespace NAUReviewApplication.Models
{
    public partial class Category
    {
        public Category()
        {
            Question = new HashSet<Question>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Question> Question { get; set; }
    }
}
