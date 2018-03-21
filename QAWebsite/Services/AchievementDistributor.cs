using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.Enums;

namespace QAWebsite.Services
{
    public class AchievementDistributor : IAchievementDistributor
    {
        public void check(string userId, ApplicationDbContext dBContext, AchievementType type)
        {
            var userAchievementIdList = dBContext.UserAchievements.Where(UserAchievement => UserAchievement.UserId == userId).Select(UserAchievement => UserAchievement.AchievementId).ToList();

            var unobtainedAchievementList = dBContext.Achievement.Where(achievement => achievement.Type == type && !userAchievementIdList.Contains(achievement.Id))
                                            .OrderBy(achievement => achievement.Threshold).ToList();
            int count = 0;
            switch (type)
            {
                case AchievementType.QuestionCreation:
                    {
                        count = dBContext.Question.Where(question => question.AuthorId == userId).Count();
                        break;
                    }
                case AchievementType.AnswerCreation:
                    {
                        count = dBContext.Answer.Where(answer => answer.AuthorId == userId).Count();
                        break;
                    }
                case AchievementType.QuestionEditing:
                    {
                        count = dBContext.QuestionEdits.Where(questionEdit => questionEdit.EditorId == userId).Count();
                        break;
                    }
            }

            foreach (var achievement in unobtainedAchievementList)
            {
                var earned = false;
                switch (achievement.Comparator)
                {
                    case Comparators.greaterThanEquals:
                        {
                            if (count >= achievement.Threshold)
                            {
                                earned = true;
                            }
                            break;
                        }
                    case Comparators.lessThanEquals:
                        {
                            if (count <= achievement.Threshold)
                            {
                                earned = true;
                            }
                            break;
                        }
                }

                if(earned)
                dBContext.UserAchievements.Add(new ApplicationUserAchievements {UserId = userId, AchievementId = achievement.Id});
            }

            dBContext.SaveChanges();
        }
    }
}
