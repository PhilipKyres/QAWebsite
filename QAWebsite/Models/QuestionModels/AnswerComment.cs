using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionModels
{
    public class AnswerComment : Comment
    {
        [MaxLength(36)]
        [ForeignKey("AnswerId")]
        public string Answerid { get; set; }

    }
}
