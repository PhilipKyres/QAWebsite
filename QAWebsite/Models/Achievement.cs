﻿using Microsoft.EntityFrameworkCore.Query.Internal;
using QAWebsite.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models
{
    public class Achievement
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Comparators Comparator { get; set; }
        public int Threshold { get; set; }
        public byte[] AchievementImage { get; set; }
        public AchievementType Type { get; set; }


        public virtual ICollection<ApplicationUserAchievements> UserAchievements { get; set; }
    }
}
