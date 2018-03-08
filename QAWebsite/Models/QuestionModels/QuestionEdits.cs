using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionModels
{
    public class QuestionEdit
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string EditorId { get; set; }

        [ForeignKey("Question")]
        public string QuestionId { get; set; }

        
        [Display(Name = "New Title")]
        public string NewTitle { get; set; }

       
        [Display(Name = "New Content")]
        public string NewContent { get; set; }

        
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }
    }
}
