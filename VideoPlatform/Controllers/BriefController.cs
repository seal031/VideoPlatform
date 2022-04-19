using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.BL;
using VideoPlatform.DB.Entity;
using VideoPlatform.Models;
using VideoPlatform.Utils;

namespace BriefPlatform.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BriefController: ControllerBase
    {
        private readonly SelectBL selectBL;
        private readonly AddBL addBL;
        private readonly DelBL delBL;
        private readonly OperateLogHelper operateLog;
        public BriefController(SelectBL _selectBL, AddBL _addBL, DelBL _delBL, OperateLogHelper _operateLogHelper)
        {
            selectBL = _selectBL;
            addBL = _addBL;
            delBL = _delBL;
            operateLog = _operateLogHelper;
        }

        [HttpGet]
        [Route("GetBriefBaseList")]
        public string getBriefBaseList([FromQuery] BriefQueryParams queryParams)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var list0 = selectBL.GetBriefList(queryParams);
                //分页前获取总数
                int totalCount = list0.ToList().Count;
                IEnumerable<BriefEntity> list = list0.AsEnumerable(); ;
                //分页
                if (queryParams.pageSize.HasValue && queryParams.pageIndex.HasValue)
                {
                    list = list0.Skip((int)((queryParams.pageIndex - 1) * queryParams.pageSize)).Take((int)queryParams.pageSize);
                }
                //var Brief_list = selectBL.GetBriefList();
                var Brief_list = (from b in list
                                 join c in selectBL.GetGlobalCodes().Where(c => c.parent_code == "05")
                                on b.brief_type equals c.code_id
                                orderby b.create_time  descending
                                select new BriefEntity
                                {
                                    brief_title = b.brief_title,
                                    brief_id = b.brief_id,
                                    create_time = b.create_time,
                                    brief_type = c.code_name
                                }).ToList();
                response.data = new { BriefList = JsonConvert.SerializeObject(Brief_list), totalCount = totalCount };
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBriefList")]
        public string getBriefList([FromQuery] BriefQueryParams queryParams)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var list0 = selectBL.GetBriefList(queryParams);
                //分页前获取总数
                int totalCount = list0.ToList().Count;
                IEnumerable<BriefEntity> list = list0.AsEnumerable(); ;
                //分页
                if (queryParams.pageSize.HasValue && queryParams.pageIndex.HasValue)
                {
                    list = list0.Skip((int)((queryParams.pageIndex - 1) * queryParams.pageSize)).Take((int)queryParams.pageSize);
                }
                var Brief_list = list.ToList();
                response.data = new { BriefList = JsonConvert.SerializeObject(Brief_list), totalCount = totalCount };
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBriefById")]
        public string getBriefById(string brief_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var Brief = selectBL.GetBriefById(brief_id);
                string typeName = string.Empty;
                var typeEntity = selectBL.GetGlobalCodes().FirstOrDefault(c => c.code_id == Brief.brief_type);
                if (typeEntity != null)
                {
                    typeName = typeEntity.code_name;
                }
                if (Brief != null)
                {
                    response.data = JsonConvert.SerializeObject(new { 
                        Brief.brief_id,
                        Brief.brief_title,
                        Brief.brief_content,
                        brief_type= typeName,
                        Brief.brief_state,
                        Brief.brief_image,
                        Brief.operate_admin,
                        Brief.create_time
                    });
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关简报");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("AddBrief")]
        public string addBrief(BriefEntity brief)
        {
            BaseResponse response = new BaseResponse(); ;
            try
            {
                brief.brief_id = brief.brief_id ?? Guid.NewGuid().ToString();
                brief.create_time = (brief.create_time == DateTime.MinValue ? DateTime.Now : brief.create_time);
                var result = addBL.AddBrief(brief);
                if (result == "success")
                {
                    operateLog.writeLog(brief.admin_id, brief.admin_ip, $"编辑了内容 {brief.toJson()}");
                    response.data = result;
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("500", result);
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("DelBrief")]
        public string delBrief(string brief_id,string brief_title,string admin_id,string admin_ip)
        {
            BaseResponse response = new BaseResponse(); ;
            try
            {
                var result = delBL.DelBrief(brief_id);
                if (result == "success")
                {
                    operateLog.writeLog(admin_id, admin_ip, $"删除了内容，标题为《{brief_title}》");
                    response.data = result;
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("500", result);
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
    }
}
