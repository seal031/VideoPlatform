using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Log4net;
using VideoPlatform.BL;
using VideoPlatform.DB.Entity;
using VideoPlatform.Models;
using VideoPlatform.Utils;

namespace VideoPlatform.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly SelectBL selectBL;
        private readonly AddBL addBL;
        private readonly DelBL delBL;
        private readonly OperateLogHelper operateLog;
        public VideoController(SelectBL _selectBL, AddBL _addBL, DelBL _delBL, OperateLogHelper _operateLogHelper)
        {
            selectBL = _selectBL;
            addBL = _addBL;
            delBL = _delBL;
            operateLog = _operateLogHelper;
        }

        [HttpPost]
        [Route("AddViewHistory")]
        public string addViewHistory(string user_id, string video_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                VideoEntity video = selectBL.GetVideoById(video_id);
                if (video != null)
                {
                    string result= addBL.AddViewHistory(user_id, video_id, video);
                    if (result == "success")
                    {
                        response.data = result;
                        return response.toJson();
                    }
                    else
                    {
                        return BaseResponse.makeAbnormalResponse2str("500", result);
                    }
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAppreciate")]
        public string addAppreciate(string user_id, string video_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (selectBL.ExistAppreciate(user_id, video_id) == false) //没赞过才可以赞，防止刷赞
                {
                    VideoEntity video = selectBL.GetVideoById(video_id);
                    if (video != null)
                    {
                        response.data = addBL.AddtAppreciate(user_id, video_id, video);
                        return response.toJson();
                    }
                    else
                    {
                        return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                    }
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("601", "每个视频只能点赞一次");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("AddCollection")]
        public string addCollection(string user_id, string video_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (selectBL.ExistCollection(user_id, video_id) == false) //没收藏过才可以收藏
                {
                    VideoEntity video = selectBL.GetVideoById(video_id);
                    if (video != null)
                    {
                        response.data = addBL.AddCollection(user_id, video_id, video);
                        return response.toJson();
                    }
                    else
                    {
                        return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                    }
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("601", "每个视频只能收藏一次");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("CancelAppreciate")]
        public string cancelAppreciate(string user_id, string video_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                VideoEntity video = selectBL.GetVideoById(video_id);
                if (video != null)
                {
                    response.data = delBL.DelAppreciate(user_id, video_id, video);
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("CancelCollection")]
        public string cancelCollection(string user_id, string video_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                VideoEntity video = selectBL.GetVideoById(video_id);
                if (video != null)
                {
                    response.data = delBL.DelCollection(user_id, video_id, video);
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
        [HttpPost]
        [Route("GetCollectionAppreciateState")]
        public string getCollectionAppreciateState(string user_id, string video_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                VideoEntity video = selectBL.GetVideoById(video_id);
                if (video != null)
                {
                    var states=selectBL.GetCollectionAppreciateState(user_id, video_id);
                    response.data = new { Collection = states.Item1, Appreciate = states.Item2 };
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetVideoList")]
        public string getVideoList([FromQuery] VideoQueryParams queryParams)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var list0 = selectBL.GetVideoList(queryParams);
                //分页前获取总数
                int totalCount = list0.ToList().Count;
                IEnumerable<VideoEntity> list = list0.AsEnumerable(); ;
                //分页
                if (queryParams.pageSize.HasValue && queryParams.pageIndex.HasValue)
                {
                    list = list0.Skip((int)((queryParams.pageIndex - 1) * queryParams.pageSize)).Take((int)queryParams.pageSize);
                }
                var video_list_0 = (from v in list
                                    join videotype in selectBL.GetGlobalCodes().Where(c => c.parent_code == "02")
                                    on v.video_type equals videotype.code_id into temp
                                    from tt in temp.DefaultIfEmpty()
                                    select new VideoEntity
                                    {
                                        video_title = v.video_title,
                                        video_id = v.video_id,
                                        video_school = v.video_school,
                                        video_type = tt == null ? "" : tt.code_name,
                                        public_type = v.public_type,
                                        teacher = v.teacher,
                                        video_year = v.video_year,
                                        create_time = v.create_time,
                                        video_state = (v.video_state == "0401" ? "已发布" : "草稿"),
                                        video_facede = v.video_facede,
                                        award = v.award,
                                        uploader = v.uploader,
                                        view_count = v.view_count,
                                        appreciate_count = v.appreciate_count,
                                        collection_count = v.collection_count,
                                    });
                var video_list1 = (from v in video_list_0
                                   join publictype in selectBL.GetGlobalCodes().Where(c => c.parent_code == "03")
                                   on v.public_type equals publictype.code_id into temp
                                   from tt in temp.DefaultIfEmpty()
                                   orderby v.create_time descending
                                   select new VideoEntity
                                   {
                                       video_title = v.video_title,
                                       video_id = v.video_id,
                                       video_school = v.video_school,
                                       public_type = tt == null ? "" : tt.code_name,
                                       video_type = v.video_type,
                                       teacher = v.teacher,
                                       video_year = v.video_year,
                                       create_time = v.create_time,
                                       video_state = v.video_state,
                                       video_facede = v.video_facede,
                                       award = v.award,
                                       uploader = v.uploader,
                                       view_count = v.view_count,
                                       appreciate_count = v.appreciate_count,
                                       collection_count = v.collection_count,
                                   });
                var video_list2 = (from v in video_list1
                                   join user in selectBL.getUserList()
                                   on v.uploader equals user.user_id into temp
                                   from tt in temp.DefaultIfEmpty()
                                   select new VideoEntity
                                   {
                                       video_title = v.video_title,
                                       video_id = v.video_id,
                                       video_school = v.video_school,
                                       public_type = v.public_type,
                                       video_type = v.video_type,
                                       teacher = v.teacher,
                                       video_year = v.video_year,
                                       create_time = v.create_time,
                                       video_state = v.video_state,
                                       video_facede = v.video_facede,
                                       award = v.award,
                                       uploader = tt == null ? "" : tt.user_name,
                                       view_count = v.view_count,
                                       appreciate_count = v.appreciate_count,
                                       collection_count = v.collection_count,
                                   }
                                   );
                var video_list = (from v in video_list2
                                  join school in selectBL.GetSchoolList(new SchoolQueryParams())
                                  on v.video_school equals school.school_id into temp
                                  from tt in temp.DefaultIfEmpty()
                                  select new VideoEntity()
                                  {
                                      video_title = v.video_title,
                                      video_id = v.video_id,
                                      video_school = tt == null ? "" : tt.school_name,
                                      public_type = v.public_type,
                                      video_type = v.video_type,
                                      teacher = v.teacher,
                                      video_year = v.video_year,
                                      create_time = v.create_time,
                                      video_state = v.video_state,
                                      video_facede = v.video_facede,
                                      award = v.award,
                                      uploader = v.uploader,
                                      view_count = v.view_count,
                                      appreciate_count = v.appreciate_count,
                                      collection_count = v.collection_count,
                                  }).ToList();
                response.data = new { videoList = JsonConvert.SerializeObject(video_list), totalCount = totalCount };
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetVideoById")]
        public string getVideoById(string video_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var video = selectBL.GetVideoById(video_id);
                if (video != null)
                {
                    response.data = JsonConvert.SerializeObject(video);
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRelativeVideoList")]
        public string getRelativeVideoList([FromQuery] VideoQueryParams queryParams)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var video = selectBL.GetVideoById(queryParams.videoId);
                if (video != null)
                {
                    var video_list = selectBL.GetVideoList(queryParams);
                    response.data = new
                    {
                        videoList = JsonConvert.SerializeObject(video_list)
                    };
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关视频");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCollectVideoList")]
        public string getCollectVideoList([FromQuery] VideoQueryParams queryParams)
        {
            //查询收藏只需要这几个字段，new一个新参数对象，防止传进来的参数带额外干扰内容
            VideoQueryParams videoQuery = new VideoQueryParams() 
            { 
                userId=queryParams.userId,
                pageSize=queryParams.pageSize,
                pageIndex=queryParams.pageIndex,
            };
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(videoQuery.userId))
                {
                    return BaseResponse.makeAbnormalResponse2str("500", "无法确定用户身份");
                }
                if (selectBL.getUserById(videoQuery.userId) == null) 
                {
                    return BaseResponse.makeAbnormalResponse2str("500", "未找到当前用户");
                }
                
                var list0 = from c in selectBL.GetCollectionsByUser(videoQuery.userId)
                           join v in selectBL.GetVideoList(videoQuery)
                           on c.video_id equals v.video_id into temp
                           from tt in temp.DefaultIfEmpty()
                           orderby c.collect_time descending //收藏时间倒序
                           select new VideoEntity
                           {
                               video_title = tt.video_title,
                               video_id = tt.video_id,
                               video_school = tt.video_school,
                               teacher = tt.teacher,
                               video_year = tt.video_year,
                               create_time = tt.create_time,
                               video_state = (tt.video_state == "0401" ? "已发布" : "草稿"),
                               video_facede = tt.video_facede,
                               award = tt.award,
                               uploader = tt.uploader,
                               view_count = tt.view_count,
                               appreciate_count = tt.appreciate_count,
                               collection_count = tt.collection_count,
                           };
                //分页前获取总数
                int totalCount = list0.ToList().Count;
                IEnumerable<VideoEntity> list = list0.AsEnumerable(); ;
                //分页
                if (queryParams.pageSize.HasValue && queryParams.pageIndex.HasValue)
                {
                    list = list0.Skip((int)((queryParams.pageIndex - 1) * queryParams.pageSize)).Take((int)queryParams.pageSize);
                }
                var video_list = (from v in list
                                  join school in selectBL.GetSchoolList(new SchoolQueryParams())
                                  on v.video_school equals school.school_id into temp
                                  from tt in temp.DefaultIfEmpty()
                                  select new VideoEntity()
                                  {
                                      video_title = v.video_title,
                                      video_id = v.video_id,
                                      video_school = tt == null ? "" : tt.school_name,
                                      teacher = v.teacher,
                                      video_year = v.video_year,
                                      create_time = v.create_time,
                                      video_state = v.video_state,
                                      video_facede = v.video_facede,
                                      award = v.award,
                                      uploader = v.uploader,
                                      view_count = v.view_count,
                                      appreciate_count = v.appreciate_count,
                                      collection_count = v.collection_count,
                                  }).ToList();
                response.data = new { videoList = JsonConvert.SerializeObject(video_list), totalCount = totalCount };
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
    }
}
