using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.DB;
using VideoPlatform.DB.Entity;

namespace VideoPlatform.BL
{
    public class AddBL
    {
        private MysqlDbContext dbContext;
        public AddBL(MysqlDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        #region 浏览记录
        public string AddViewHistory(string user_id, string video_id,VideoEntity video)
        {
            if (string.IsNullOrEmpty(user_id))//如果没有user_id，说明是游客观看，此时不用记录观看历史，只增加视频浏览量
            {
                try
                {
                    video.view_count += 1;
                    dbContext.SaveChanges();
                    return "success";
                }
                catch (Exception ex)
                {
                    return "success";//游客浏览量增加不成功也无所谓
                }
            }
            else
            {
                ViewHistoryEntity viewHistory = new ViewHistoryEntity()
                {
                    user_id = user_id,
                    video_id = video_id,
                    view_time = DateTime.Now
                };
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.ViewHistorys.Add(viewHistory);
                        video.view_count += 1;
                        dbContext.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return ex.Message;
                    }
                }
            }
        }
        #endregion

        #region 点赞记录
        public string AddtAppreciate(string user_id, string video_id, VideoEntity video)
        {
            AppreciateEntity appreciate = new AppreciateEntity()
            {
                user_id = user_id,
                video_id = video_id,
                appreciate_time = DateTime.Now
            };
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Appreciates.Add(appreciate);
                    video.appreciate_count += 1;
                    dbContext.SaveChanges();
                    transaction.Commit();
                    return "success";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ex.Message;
                }
            }
        }
        #endregion

        #region 收藏记录
        public string AddCollection(string user_id, string video_id, VideoEntity video)
        {
            CollectEntity collection = new CollectEntity()
            {
                user_id = user_id,
                video_id = video_id,
                collect_time = DateTime.Now
            };
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Collectionses.Add(collection);
                    video.collection_count += 1;
                    dbContext.SaveChanges();
                    transaction.Commit();
                    return "success";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ex.Message;
                }
            }
        }
        #endregion

        #region 学校
        public string AddSchool(SchoolEntity school)
        {
            try
            {
                var exist = dbContext.Schools.FirstOrDefault(s => s.school_id == school.school_id);
                if (exist == null)
                {
                    dbContext.Schools.Add(school);
                }
                else
                {
                    exist = school;
                }
                dbContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 用户
        internal string AddUser(UserEntity user)
        {
            try
            {
                var exist = dbContext.Users.FirstOrDefault(u => u.user_id == user.user_id);
                if (exist == null)
                {
                    dbContext.Users.Add(user);
                }
                else
                {
                    exist = user;
                }
                dbContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 公共编码
        public string AddGlobalCode(GlobalCodeEntity globalCode)
        {
            try
            {
                var exist = dbContext.GlobalCodes.FirstOrDefault(c => c.code_id == globalCode.code_id);
                if (exist == null)
                {
                    dbContext.GlobalCodes.Add(globalCode);
                }
                else
                {
                    exist = globalCode;
                }
                dbContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 简报
        public string AddBrief(BriefEntity brief)
        {
            try
            {
                var exist = dbContext.Briefs.FirstOrDefault(b => b.brief_id == brief.brief_id);
                if (exist == null)
                {
                    dbContext.Briefs.Add(brief);
                }
                else
                {
                    exist = brief;
                }
                dbContext.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
