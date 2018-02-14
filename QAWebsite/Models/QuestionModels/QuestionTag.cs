using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QAWebsite.Models.QuestionModels
{
    public class QuestionTag
    {
        [MaxLength(8)]
        public string QuestionId { get; set; }
        public virtual Question Question { get; set; }

        [MaxLength(36)]
        public string TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
