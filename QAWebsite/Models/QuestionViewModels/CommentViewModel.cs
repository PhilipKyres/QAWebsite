using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment, string authorName, string parentId, CommentTypes type)
        {
            this.Id = comment.Id;
            this.Content = comment.Content;
            this.CreationDate = comment.CreationDate;
            this.AuthorId = comment.AuthorId;
            this.AuthorName = authorName;
            this.ParentId = parentId;
            this.Type = type;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [MaxLength(450)]
        [Display(Name = "Author")]
        public string AuthorName { get; set; }

        [Required]
        public string ParentId { get; set; }

        [Required]
        public CommentTypes Type { get; set; }
    }
}
