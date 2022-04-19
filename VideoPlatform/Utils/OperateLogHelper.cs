using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.DB;
using VideoPlatform.DB.Entity;

namespace VideoPlatform.Utils
{
    public class OperateLogHelper
    {
        MysqlDbContext dbContext;
        public OperateLogHelper(MysqlDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public  void writeLog(string user_id, string user_ip, string log)
        {
            OperateLogEntity operateLog = new OperateLogEntity()
            {
                user_id = user_id,
                user_ip=user_ip,
                operate_content=log,
                operate_time=DateTime.Now
            };
            try
            {
                dbContext.OperateLogs.Add(operateLog);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
