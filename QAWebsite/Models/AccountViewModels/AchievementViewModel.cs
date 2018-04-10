using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.AccountViewModels
{
    public class AchievementViewModel
    {
        public string Username { get; set; }
        public IEnumerable<AchievementDisplayContainer> Achievements { get; set; }
    }

    public class AchievementDisplayContainer
    {
        public string Image { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}
