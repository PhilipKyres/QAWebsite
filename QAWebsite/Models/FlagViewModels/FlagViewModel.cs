using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QAWebsite.Models.FlagViewModels
{
    public class FlagViewModel
    {
        public FlagViewModel() {}

        public FlagViewModel(Question question)
        {
            this.Id = question.Id;
        }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Reason")]
        public string SelectedReason { get; set; }
        public List<SelectListItem> Reasons { get; } = new List<SelectListItem> 
        {
            new SelectListItem { Value = "0", Text = "Duplicate" },
            new SelectListItem { Value = "1", Text = "Inappropriate" },
            new SelectListItem { Value = "2", Text = "Off Topic" },
            new SelectListItem { Value = "3", Text = "Other" }
        };

        [Required]
        public string Content { get; set; }
    }
}
