using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class AnswerComment : Comment
    {
        [MaxLength(36)]
        [ForeignKey("AnswerId")]
        public string Answerid { get; set; }
    }
}
