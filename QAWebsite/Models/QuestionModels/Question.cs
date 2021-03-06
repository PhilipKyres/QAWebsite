﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QAWebsite.Models.UserModels;

namespace QAWebsite.Models.QuestionModels
{
    public class Question
    {
        [Key]
        [MaxLength(8)]
        public string Id { get; set; }

        [Required]
        [MaxLength(300)]
        [MinLength(15)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

        [MaxLength(450)]
        [ForeignKey("ApplicationUser")]
        public string AuthorId { get; set; }

        [MaxLength(36)]
        [ForeignKey("Answer")]
        public string BestAnswerId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<QuestionComment> Comments { get; set; }

        public virtual ICollection<Flag> Flags { get; set; }
    }
}
