using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.MultipleViewModels
{
    public class QuestionTagsViewModel
    {
        public IEnumerable<Question> QuestionModel { get; set; }

        public IEnumerable<TaggingViewModel> TagsModel { get; set; }
    }
}
