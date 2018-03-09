using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class QuestionComment : Comment
    {
        [MaxLength(36)]
        [ForeignKey("Question")]
        public string QuestionId { get; set; }
    }
}
