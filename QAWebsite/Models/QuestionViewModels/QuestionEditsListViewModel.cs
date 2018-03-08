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
        [Required]
        public string QuestionId { get; set; }

        public ICollection<QuestionEditListItem> Edits { get; set; }
    }

    public class QuestionEditListItem{
        public QuestionEditListItem(QuestionEdit edit, string editorName)
        {
            this.EditId = edit.Id;
            this.EditedBy = editorName;
            this.EditorId = edit.EditorId;
            this.EditDate = edit.EditDate;
            this.TitleChanged = edit.NewTitle != null;
            this.ContentChanged = edit.NewContent != null;
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
