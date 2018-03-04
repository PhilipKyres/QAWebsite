using QAWebsite.Models.QuestionModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionViewModels
{
    public class QuestionEditsListViewModel
    {
        public QuestionEditsListViewModel(QuestionEdits edit, string editorName)
        {
            this.EditId = edit.Id;
            this.EditedBy = editorName;
            this.EditorId = edit.EditorId;
            this.EditDate = edit.EditDate;
            this.TitleChanged = edit.newTitle != null;
            this.ContentChanged = edit.newContent != null;
        }

        [Required]
        public string EditId { get; set; }

        [Required]
        [Display(Name = "Edited By")]
        public string EditedBy { get; set; }

        [Required]
        public string EditorId { get; set; }

        [Required]
        [Display(Name = "Title Changed")]
        public bool TitleChanged { get; set; }

        [Required]
        [Display(Name = "Content Changed")]
        public bool ContentChanged { get; set; }

        [Required]
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }
    }
}
