using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.DB.Entity;

namespace VideoPlatform.DB
{
    public class MysqlDbContext: DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GlobalCodeEntity> GlobalCodes { get; set; }
        public DbSet<SchoolEntity> Schools { get; set; }
        public DbSet<VideoEntity> Videos { get; set; }
        public DbSet<AppreciateEntity> Appreciates { get; set; }
        public DbSet<CollectEntity> Collectionses { get; set; }
        public DbSet<ViewHistoryEntity> ViewHistorys { get; set; }
        public DbSet<OperateLogEntity> OperateLogs { get; set; }
        public DbSet<BriefEntity> Briefs { get; set; }
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options)
        {

        }
    }
}
