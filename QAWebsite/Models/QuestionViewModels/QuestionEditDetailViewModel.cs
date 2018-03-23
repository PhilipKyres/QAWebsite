using QAWebsite.Models.QuestionModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace QAWebsite.Models.QuestionViewModels
{
    public class QuestionEditDetailViewModel
    {
        public QuestionEditDetailViewModel(QuestionEdit edit, string initialTitle, string initialContent, string editorName)
        {
            this.EditId = edit.Id;
            this.EditedBy = editorName;
            this.EditorId = edit.EditorId;
            this.EditDate = edit.EditDate;
            this.InitialContent = initialContent;
            this.NewContent = edit.NewContent;
            this.InitialTitle = initialTitle;
            this.NewTitle= edit.NewTitle;
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
        [Display(Name = "New Content")]
        public String NewContent { get; set; }

        [Required]
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

    }
}
