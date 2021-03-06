﻿using System.Linq;
using QAWebsite.Data;
using QAWebsite.Models.Enums;
using QAWebsite.Models.UserModels;

namespace QAWebsite.Services
{
    public class AchievementDistributor : IAchievementDistributor
    {
        public void check(string userId, ApplicationDbContext dBContext, AchievementType type)
        {
            var userAchievementIdList = dBContext.UserAchievements.Where(userAchievement => userAchievement.UserId == userId).Select(userAchievement => userAchievement.AchievementId).ToList();

            var unobtainedAchievementList = dBContext.Achievement.Where(achievement => achievement.Type == type && !userAchievementIdList.Contains(achievement.Id))
                                            .OrderBy(achievement => achievement.Threshold).ToList();
            int count = 0;
            switch (type)
            {
                case AchievementType.QuestionCreation:
                    {
                        count = dBContext.Question.Count(question => question.AuthorId == userId);
                        break;
                    }
                case AchievementType.AnswerCreation:
                    {
                        count = dBContext.Answer.Count(answer => answer.AuthorId == userId);
                        break;
                    }
                case AchievementType.QuestionEditing:
                    {
                        count = dBContext.QuestionEdits.Count(questionEdit => questionEdit.EditorId == userId);
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
