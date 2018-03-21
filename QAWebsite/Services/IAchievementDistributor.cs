using QAWebsite.Data;
using QAWebsite.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Services
{
    public interface IAchievementDistributor
    {
        void check(string UserId, ApplicationDbContext dBContext, AchievementType type);
    }
}
