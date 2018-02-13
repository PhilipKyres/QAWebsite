﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QAWebsite.Models
{
    public class Flag
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Question Id")]
        public string QuestionId { get; set; }

        [Required]
        [Display(Name = "Reason")]
        public string Reason { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

    }
}
