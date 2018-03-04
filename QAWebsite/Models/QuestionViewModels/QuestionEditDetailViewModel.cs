using QAWebsite.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionViewModels
{
    public class QuestionEditDetailViewModel
    {
        public QuestionEditDetailViewModel(QuestionEdits edit, string editorName)
        {
            this.EditId = edit.Id;
            this.EditedBy = editorName;
            this.EditorId = edit.EditorId;
            this.EditDate = edit.EditDate;
            this.InitialContent = edit.initialContent;
            this.NewContent = edit.newContent;
            this.InitialTitle = edit.initialTitle;
            this.NewTitle= edit.newTitle;
            this.QuestionId = edit.QuestionId;
        }

        [Required]
        public string EditId { get; set; }

        [Required]
        [Display(Name = "Edited By")]
        public string EditedBy { get; set; }


        [Required]
        public string QuestionId { get; set; }

        [Required]
        public string EditorId { get; set; }

        [Required]
        [Display(Name = "Initial Title")]
        public String InitialTitle { get; set; }

        [Required]
        [Display(Name = "New Title")]
        public String NewTitle { get; set; }

        [Required]
        [Display(Name = "Initial Content")]
        public String InitialContent { get; set; }

        [Required]
        [Display(Name = "Initial Content")]
        public String NewContent { get; set; }

        [Required]
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

    }
}
