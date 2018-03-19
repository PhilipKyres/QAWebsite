using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class QuestionComment : Comment
    {
        [ForeignKey("Answer")]
        public override string FkId { get; set; }

        [Required]
        public virtual Question Question { get; set; }
    }
}
