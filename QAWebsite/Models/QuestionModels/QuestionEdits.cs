using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionModels
{
    public class QuestionEdits
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string EditorId { get; set; }

        [ForeignKey("Question")]
        public string QuestionId { get; set; }

        
        [Display(Name = "Initial Title")]
        public string initialTitle { get; set; }

        
        [Display(Name = "New Title")]
        public string newTitle { get; set; }

        
        [Display(Name = "Initial Content")]
        public string initialContent { get; set; }

       
        [Display(Name = "New Content")]
        public string newContent { get; set; }

        
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }
    }
}
