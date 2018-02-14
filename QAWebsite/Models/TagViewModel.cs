using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models
{
    public class TagViewModel
    {
        public TagViewModel(Tag tag, int count)
        {
            Id = tag.Id;
            Name = tag.Name;
            Count = count;
        }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Tag")]
        public string Name { get; set; }

        [Display(Name = "Questions Tagged")]
        public int Count { get; set; }
    }
}
