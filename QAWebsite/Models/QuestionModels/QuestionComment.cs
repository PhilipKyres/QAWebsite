using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionModels
{
    public class QuestionComment : Comment
    {
        [MaxLength(36)]
        [ForeignKey("Question")]
        public string QuestionId { get; set; }
    }
}
