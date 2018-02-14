using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QAWebsite.Models.QuestionModels
{
    public class Tag
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(35)]
        [Display(Name = "Tag Name")]
        public string Name { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
