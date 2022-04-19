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
    public class GlobalCodeController : ControllerBase
    {
        private readonly SelectBL selectBL;
        private readonly AddBL addBL;
        private readonly DelBL delBL;
        private readonly OperateLogHelper operateLog;

        public GlobalCodeController(SelectBL _selectBL, AddBL _addBL, DelBL _delBL, OperateLogHelper _operateLogHelper)
        {
            selectBL = _selectBL;
            addBL = _addBL;
            delBL = _delBL;
            operateLog = _operateLogHelper;
        }

        [HttpPost]
        [Route("Login")]
        public string login(UserEntity user)
        {
            BaseResponse response = new BaseResponse();
            if (string.IsNullOrEmpty(user.user_name) || string.IsNullOrEmpty(user.user_pwd))
            {
                return BaseResponse.makeAbnormalResponse2str("500", "用户名或密码不能为空");
            }
            else
            {
                try
                {
                    var check = selectBL.getUserList().FirstOrDefault(u => u.user_name == user.user_name && u.user_pwd == user.user_pwd);
                    if (check == null)
                    {
                        return BaseResponse.makeAbnormalResponse2str("505", "用户名或密码错误");
                    }
                    else
                    {
                       response.data= JsonConvert.SerializeObject(new { 
                            check.user_id,
                            check.user_name,
                            check.real_name,
                            check.user_school,
                            check.user_role
                       }); 
                       return response.toJson();
                    }
                }
                catch (Exception ex)
                {
                    return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetUserById")]
        public string getUserById(string user_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var user = selectBL.getUserById(user_id);
                if (user != null)
                {
                    response.data = JsonConvert.SerializeObject(user);
                    return response.toJson();
                }
                else
                {
                    return BaseResponse.makeAbnormalResponse2str("500", "未找到相关用户");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserBySchoolId")]
        public string getUserBySchoolId(string school_id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var userList = selectBL.getUserList().Where(u => u.user_school == school_id);
                response.data = new { UserList = JsonConvert.SerializeObject(userList) };
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
        [HttpGet]
        [Route("GetVideoType")]
        public string getVideoType()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var videoTypeList = selectBL.GetGlobalCodes().Where(c => c.parent_code == "02");
                response.data = videoTypeList;
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
        [HttpGet]
        [Route("GetVideoPublicType")]
        public string getVideoPublicType()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var videoTypeList = selectBL.GetGlobalCodes().Where(c => c.parent_code == "03");
                response.data = videoTypeList.ToList();
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
        [HttpGet]
        [Route("GetColumnType")]
        public string GetColumnType()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var videoTypeList = selectBL.GetGlobalCodes().Where(c => c.parent_code == "05");
                response.data = videoTypeList.ToList();// JsonConvert.SerializeObject(videoTypeList);
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
        [HttpGet]
        [Route("GetSchoolType")]
        public string getSchoolType()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var schoolTypeList = selectBL.GetGlobalCodes().Where(c => c.parent_code == "06");
                response.data = schoolTypeList.ToList();
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }
        
        [HttpGet]
        [Route("GetSchoolCategoryType")]
        public string getSchoolCategoryType()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var schoolCategoryList = selectBL.GetGlobalCodes().Where(c => c.parent_code == "07");
                response.data = schoolCategoryList.ToList();
                return response.toJson();
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public string addUser(UserEntity user)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string result = addBL.AddUser(user);
                if (result == "success")
                {
                    operateLog.writeLog(user.admin_id, user.admin_ip, $"添加了用户{user.toJson()}");
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
        [Route("DelUser")]
        public string delUser(string user_id, string admin_id, string admin_ip)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var user = selectBL.getUserById(user_id);
                if (user != null)
                {
                    var result = delBL.DelUser(user_id);
                    if (result == "success")
                    {
                        operateLog.writeLog(admin_id, admin_ip, $"删除了账号{user.toJson()}");
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
                    return BaseResponse.makeAbnormalResponse2str("500", "未找到相关账号");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
            }
        }

        [HttpPost]
        [Route("AddVideoType")]
        public string addVideoType(GlobalCodeEntity videoType)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string result = addBL.AddGlobalCode(videoType);
                if (result == "success")
                {
                    operateLog.writeLog(videoType.admin_id, videoType.admin_ip, $"添加了视频类型 {videoType.toJson()}");
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
        [Route("AddPublicType")]
        public string addPublicType(GlobalCodeEntity videoPublicType)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string result = addBL.AddGlobalCode(videoPublicType);
                if (result == "success")
                {
                    operateLog.writeLog(videoPublicType.admin_id, videoPublicType.admin_ip, $"添加了视频发布类型{videoPublicType.toJson()}");
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
        [Route("AddColumn")]
        public string addColumn(GlobalCodeEntity column)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string result = addBL.AddGlobalCode(column);
                if (result == "success")
                {
                    operateLog.writeLog(column.admin_id, column.admin_ip, $"添加了栏目{column.toJson()}");
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
        [Route("AddSchoolType")]
        public string addSchoolType(GlobalCodeEntity schoolType)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string result = addBL.AddGlobalCode(schoolType);
                if (result == "success")
                {
                    operateLog.writeLog(schoolType.admin_id, schoolType.admin_ip, $"添加了学校类型{schoolType.toJson()}");
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
