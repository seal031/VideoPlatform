using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.DB;
using VideoPlatform.DB.Entity;
using VideoPlatform.Models;

namespace VideoPlatform.BL
{
    public class SelectBL
    {
        private MysqlDbContext dbContext;
        public SelectBL(MysqlDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        #region 公共编码
        public IList<GlobalCodeEntity> GetGlobalCodes()
        {
            return dbContext.GlobalCodes.ToList();
        }
        #endregion

        #region 用户
        public IList<UserEntity> getUserList()
        {
            return dbContext.Users.ToList();
        }

        public UserEntity getUserById(string id)
        {
            return dbContext.Users.FirstOrDefault(u => u.user_id == id);
        }
        #endregion

        #region 简报
        public IList<BriefEntity> GetBriefList(BriefQueryParams queryParams)
        {
            var list = dbContext.Briefs.Where(b => b.is_deleted == 0).AsEnumerable();
            if (string.IsNullOrEmpty(queryParams.keyword) == false)
            {
                list = list.Where(b => b.brief_title.Contains(queryParams.keyword) || b.brief_content.Contains(queryParams.keyword));
            }
            if (string.IsNullOrEmpty(queryParams.briefState) == false)
            {
                list = list.Where(b => b.brief_state == queryParams.briefState);
            }
            if (string.IsNullOrEmpty(queryParams.briefType) == false)
            {
                list = list.Where(b => b.brief_type == queryParams.briefType);
            }
            if (queryParams.topN.HasValue)
            {
                list = list.Take((int)queryParams.topN);
            }
            return list.ToList();
        }

        public BriefEntity GetBriefById(string id)
        {
            return dbContext.Briefs.FirstOrDefault(b => b.brief_id == id);
        }
        #endregion

        #region 学校
        public IList<SchoolEntity> GetSchoolList(SchoolQueryParams queryParams)
        {
            var list = dbContext.Schools.Where(s => s.is_deleted == 0).AsEnumerable();
            if (string.IsNullOrEmpty(queryParams.schoolType) == false)
            {
                list = list.Where(s => s.school_type_code == queryParams.schoolType);
            }
            if (string.IsNullOrEmpty(queryParams.schoolCategoly) == false)
            {
                list = list.Where(s => s.school_category_code == queryParams.schoolCategoly);
            }
            if (string.IsNullOrEmpty(queryParams.keyword) == false)
            {
                list = list.Where(s => s.school_name.Contains(queryParams.keyword));
            }
            return list.OrderBy(s=>s.school_name).ToList();
        }

        public SchoolEntity GetSchoolById(string id)
        {
            return dbContext.Schools.FirstOrDefault(s => s.school_id == id);
        }
        #endregion

        #region 视频
        public IList<VideoEntity> GetVideoList(VideoQueryParams queryParams)
        {
            var listPublic = dbContext.Videos.Where(v => v.is_deleted == 0 && v.public_type == "0301");//未删除的公共视频，大家都能看
            var list = dbContext.Videos.Where(v => v.is_deleted == 0 && v.public_type != "0301");//未删除的非公共视频，就需要权限判断了
            if (string.IsNullOrEmpty(queryParams.userRole)) //如果角色为空，说明是非登录用户，只能看公开视频
            {
                list = list.Where(v => v.public_type == "0301");//0301公众开放
            }
            else
            {
                switch (queryParams.userRole)
                {
                    case "0101"://超管，除了已删除全能看，不做过滤

                        break;
                    case "0102"://高校管理员，公开视频+本校视频+对此校开放的视频
                        list = list.Where(v => v.public_type == "0301"||v.video_school==queryParams.videoSchool||v.public_school.Contains(queryParams.publicSchool));
                        break;
                    case "0103"://注册用户，公开视频+本校视频+对此校开放的视频
                        list = list.Where(v => v.public_type == "0301" || v.video_school == queryParams.videoSchool || v.public_school.Contains(queryParams.publicSchool));
                        break;
                    default:
                        break;
                }
            }
            if (string.IsNullOrEmpty(queryParams.keyword) == false)//关键字，公共视频也要参与筛选
            {
                list = list.Where(v => v.video_title.Contains(queryParams.keyword)||v.video_brief.Contains(queryParams.keyword));
                listPublic = listPublic.Where(v => v.video_title.Contains(queryParams.keyword) || v.video_brief.Contains(queryParams.keyword));
            }
            if (string.IsNullOrEmpty(queryParams.publicType) == false)
            {
                list = list.Where(v => v.public_type == queryParams.publicType);
            }
            if (string.IsNullOrEmpty(queryParams.videoState) == false)
            {
                list = list.Where(v => v.video_state == queryParams.videoState);
            }
            if (string.IsNullOrEmpty(queryParams.videoType) == false)//视频类型，公共视频也要参与筛选
            {
                list = list.Where(v => v.video_type == queryParams.videoType);
                listPublic = listPublic.Where(v => v.video_type == queryParams.videoType);
            }
            if (queryParams.videoYear.HasValue)//关键字，公共视频也要参与筛选
            {
                list = list.Where(v => v.video_year == queryParams.videoYear);
                listPublic = listPublic.Where(v => v.video_year == queryParams.videoYear);
            }
            if (string.IsNullOrEmpty(queryParams.videoSchool) == false)//视频上传学校
            {
                list = list.Where(v => v.video_school == queryParams.videoSchool);
            }
            if (string.IsNullOrEmpty(queryParams.uploader) == false)
            {
                list = list.Where(v => v.uploader == queryParams.uploader);
            }
            list = list.Union(listPublic);//合并公共视频和经过权限筛选的非公共视频
            if (queryParams.orderBy == "hot")//hot表示按浏览次数排序，参数无值表示默认按创建时间排序
            {
                list = list.OrderByDescending(v => v.view_count);
            }
            else
            {
                list = list.OrderByDescending(v => v.create_time);
            }
            if (queryParams.topN.HasValue)//前几，用于首页展示
            {
                list = list.Take((int)queryParams.topN);
            }
            return list.ToList();
        }

        public VideoEntity GetVideoById(string id)
        {
            return dbContext.Videos.FirstOrDefault(v => v.video_id == id);
        }
        #endregion

        #region 浏览记录
        public IList<ViewHistoryEntity> GetViewHistoriesByUser(string user_id)
        {
            return dbContext.ViewHistorys.Where(vh=>vh.user_id==user_id).ToList();
        }

        public IList<ViewHistoryEntity> GetViewHistoriesByVideo(string video_id)
        {
            return dbContext.ViewHistorys.Where(vh => vh.video_id == video_id).ToList();
        }
        #endregion

        #region 点赞记录
        public IList<AppreciateEntity> GetAppreciatesByUser(string user_id)
        {
            return dbContext.Appreciates.Where(a => a.user_id == user_id).ToList();
        }

        public IList<AppreciateEntity> GetAppreciatesByVideo(string video_id)
        {
            return dbContext.Appreciates.Where(a => a.video_id == video_id).ToList();
        }

        public bool ExistAppreciate(string user_id, string video_id)
        {
            return dbContext.Appreciates.FirstOrDefault(a => a.user_id == user_id && a.video_id == video_id) != null;
        }
        #endregion

        #region 收藏记录
        public IList<CollectEntity> GetCollectionsByUser(string user_id)
        {
            return dbContext.Collectionses.Where(c => c.user_id == user_id).ToList();
        }

        public IList<CollectEntity> GetCollectionsByVide(string video_id)
        {
            return dbContext.Collectionses.Where(c => c.video_id == video_id).ToList();
        }
        public bool ExistCollection(string user_id, string video_id)
        {
            return dbContext.Collectionses.FirstOrDefault(a => a.user_id == user_id && a.video_id == video_id) != null;
        }
        #endregion

        public (bool,bool) GetCollectionAppreciateState(string user_id, string video_id)
        {
            bool collection = dbContext.Collectionses.FirstOrDefault(c => c.user_id == user_id && c.video_id == video_id) != null;
            bool appreciate = dbContext.Appreciates.FirstOrDefault(a => a.user_id == user_id && a.video_id == video_id) != null;
            return (collection, appreciate);
        }
    }
}
