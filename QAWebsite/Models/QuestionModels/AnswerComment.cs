using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class AnswerComment : Comment
    {
        [Required]
        public virtual Answer Answer { get; set; }

        [ForeignKey("Answer")]
        public override string FkId { get; set; }
    }
}
