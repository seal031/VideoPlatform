using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.DB;
using VideoPlatform.DB.Entity;

namespace VideoPlatform.BL
{
    public class DelBL
    {
        private MysqlDbContext dbContext;

        public DelBL(MysqlDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        #region 取消点赞
        public string DelAppreciate(string user_id, string video_id, VideoEntity video)
        {
            AppreciateEntity appreciate = dbContext.Appreciates.FirstOrDefault(v => v.user_id == user_id && v.video_id == video_id);
            if (appreciate != null)
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.Appreciates.Remove(appreciate);
                        video.view_count -= 1;
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
            else
            {
                return "没有点赞记录";
            }
        }
        #endregion

        #region 取消收藏
        public string DelCollection(string user_id, string video_id, VideoEntity video)
        {
            CollectEntity collection = dbContext.Collectionses.FirstOrDefault(v => v.user_id == user_id && v.video_id == video_id);
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Collectionses.Remove(collection);
                    video.collection_count -= 1;
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

        #region 删除学校
        internal string DelSchoolById(string school_id)
        {
            SchoolEntity school = dbContext.Schools.FirstOrDefault(s => s.school_id == school_id);
            try
            {
                if (school != null)
                {
                    //dbContext.Schools.Remove(school);
                    school.is_deleted = 1;
                    dbContext.SaveChanges();
                    return "success";
                }
                else
                {
                    return "未找到相关学校";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 删除用户
        public string DelUser(string user_id)
        {
            UserEntity user = dbContext.Users.FirstOrDefault(u => u.user_id == user_id);
            try
            {
                if (user != null)
                {
                    //dbContext.Users.Remove(user);
                    user.is_deleted = 1;
                    dbContext.SaveChanges();
                    return "success";
                }
                else
                {
                    return "未找到相关用户";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 删除公共编码
        public string DelGlobalCode(string code_id)
        {
            GlobalCodeEntity globalCode = dbContext.GlobalCodes.FirstOrDefault(c => c.code_id == code_id);
            try
            {
                if (globalCode != null)
                {
                    //dbContext.GlobalCodes.Remove(globalCode);
                    globalCode.is_deleted = 1;
                    dbContext.SaveChanges();
                    return "success";
                }
                else
                {
                    return "未找到相关编码";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 删除简报
        public string DelBrief(string brief_id)
        {
            BriefEntity brief = dbContext.Briefs.FirstOrDefault(b => b.brief_id == brief_id);
            try
            {
                if (brief != null)
                {
                    //dbContext.Briefs.Remove(brief);
                    brief.is_deleted = 1;
                    dbContext.SaveChanges();
                    return "success";
                }
                else
                {
                    return "未找到相关内容";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
