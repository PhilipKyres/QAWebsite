using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models
{
    public class ApplicationUserAchievements
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [ForeignKey("Achievement")]
        public string AchievementId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Achievement Achievement { get; set; }
    }
}
