using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QAWebsite.Models.Enums
{
    public enum FlagType
    {
        [Display(Name = "Duplicate")]
        Duplicate = 0,

        [Display(Name = "Inappropriate")]
        Inappropriate = 1,

        [Display(Name = "Off Topic")]
        OffTopic = 2,

        [Display(Name = "Other")]
        Other = 3
    }
}
