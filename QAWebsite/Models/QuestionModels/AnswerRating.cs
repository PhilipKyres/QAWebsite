using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class AnswerRating : Rating
    {
        [ForeignKey("Answer")]
        public override string FkId { get; set; }
    }
}