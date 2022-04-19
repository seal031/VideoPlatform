using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.BL;
using VideoPlatform.DB;
using VideoPlatform.DB.Entity;
using VideoPlatform.Models;
using VideoPlatform.Utils;

namespace VideoPlatform.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly SelectBL selectBL;
        private readonly AddBL addBL;
        private readonly DelBL delBL;
        private readonly OperateLogHelper operateLog;

        public SchoolController(SelectBL _selectBL, AddBL _addBL, DelBL _delBL, OperateLogHelper _operateLogHelper)
        {
            selectBL = _selectBL;
            addBL = _addBL;
            delBL = _delBL;
            operateLog = _operateLogHelper;
        }
        [HttpGet]
        [Route("GetSchoolList")]
        public string getSchoolList([FromQuery] SchoolQueryParams queryParams)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var list0 = selectBL.GetSchoolList(queryParams);
                //分页前获取总数
                int totalCount = list0.ToList().Count;
                IEnumerable<SchoolEntity> list = list0.AsEnumerable(); ;
                //分页
                if (queryParams.pageSize.HasValue && queryParams.pageIndex.HasValue)
                {
                    list = list0.Skip((int)((queryParams.pageIndex - 1) * queryParams.pageSize)).Take((int)queryParams.pageSize);
                }
                response.data = new { School_list = JsonConvert.SerializeObject(list), totalCount= totalCount };
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSchoolById")]
        public string getSchoolById(string school_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var school = selectBL.GetSchoolById(school_id);
                if (school != null)
                {
                    response.data = JsonConvert.SerializeObject(school);
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("404", "未找到相关学校");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
        [HttpPost]
        [Route("AddSchool")]
        public string addSchool(SchoolEntity school)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var result = addBL.AddSchool(school);
                if (result == "success")
                {
                    operateLog.writeLog(school.admin_id, school.admin_ip, $"添加了添加了学校{school.toJson()}");
                    response.data = result;
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("500",result);
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("DelSchool")]
        public string delSchool(string school_id,string admin_id,string admin_ip)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var school = selectBL.GetSchoolById(school_id);
                if (school != null)
                {
                    var result = delBL.DelSchoolById(school_id);
                    if (result == "success")
                    {
                        operateLog.writeLog(admin_id, admin_ip, $"删除了学校{school.toJson()}");
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
                    return BaseResponse.makeAbnormalResponse2str("500", "未找到相关学校");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
    }
}
