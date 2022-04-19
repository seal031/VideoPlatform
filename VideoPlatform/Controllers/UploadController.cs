using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.BL;
using VideoPlatform.Models;
using VideoPlatform.Utils;

namespace VideoPlatform.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly SelectBL selectBL;
        private readonly AddBL addBL;
        private readonly DelBL delBL;
        private readonly OperateLogHelper operateLog;

        private int MaxSize = 1024 * 1024 * 10;
        private List<string> FileTypes =new List<string>(){ ".JPG" ,".PNG"};
        private string ImageBaseUrl = "Images";

        public UploadController(SelectBL _selectBL, AddBL _addBL, DelBL _delBL, OperateLogHelper _operateLogHelper)
        {
            selectBL = _selectBL;
            addBL = _addBL;
            delBL = _delBL;
            operateLog = _operateLogHelper;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">来自form表单的文件信息</param>
        /// <returns></returns>
        [HttpPost]
        public string Post([FromForm] IFormFile file)
        {
            BaseResponse response = new BaseResponse();
            if (file == null)
            {
                return BaseResponse.makeAbnormalResponse2str("500", "请选择上传的文件");//未找到文件
            }
            if (file.Length <= MaxSize)//检查文件大小
            {
                var suffix = Path.GetExtension(file.FileName).ToUpper();//提取上传的文件文件后缀
                if (FileTypes.IndexOf(suffix) >= 0)//检查文件格式
                {
                    try
                    {
                        //CombineIdHelper combineId = new CombineIdHelper();//我自己的combine id生成器
                        string combineId = Guid.NewGuid().ToString();
                        using (FileStream fs = System.IO.File.Create($@"{ImageBaseUrl}\{combineId}{suffix}"))//注意路径里面最好不要有中文
                        {
                            file.CopyTo(fs);//将上传的文件文件流，复制到fs中
                            fs.Flush();//清空文件流
                        }
                        //return StatusCode(200, new { newFileName = $"{combineId.LastId}{suffix}" });
                        response.data = new { newFileName = $"{combineId}{suffix}" };//将新文件文件名回传给前端
                        return response.toJson();
                    }
                    catch (Exception ex)
                    {
                        return BaseResponse.makeAbnormalResponse2str("500", ex.Message);
                    }
                }
                else
                    return BaseResponse.makeAbnormalResponse2str("500", "不支持此文件类型");//类型不正确
            }
            else
                return BaseResponse.makeAbnormalResponse2str("500", "文件大小不得超过");//请求体过大，文件大小超标
           //return StatusCode(413, new { msg = $"文件大小不得超过{this._pictureOptions.MaxSize / (1024f * 1024f)}M" });
        }
    }
}
