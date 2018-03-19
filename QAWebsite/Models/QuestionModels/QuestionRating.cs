﻿using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class QuestionRating : Rating
    {
        [ForeignKey("Question")]
        public override string FkId { get; set; }
    }
}