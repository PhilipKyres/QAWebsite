﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionViewModels
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [DisplayName("Tags (Comma separated)")]
        public string Tags { get; set; }
    }
}
