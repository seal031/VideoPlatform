using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.BL;
using VideoPlatform.DB;
using VideoPlatform.Utils;

namespace VideoPlatform
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Log4netHelper.Repository = LogManager.CreateRepository("NETCoreRepository");
            //XmlConfigurator.Configure(Log4netHelper.Repository, new FileInfo(Environment.CurrentDirectory + "/Config/log4net.config"));
            services.AddDbContext<MysqlDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL"), MySqlServerVersion.LatestSupportedServerVersion));
            services.AddControllers();
            services.AddScoped<SelectBL>();
            services.AddScoped<AddBL>();
            services.AddScoped<DelBL>();
            services.AddScoped<OperateLogHelper>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VideoPlatform", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VideoPlatform v1"));
                app.UseCors(cfg =>
                {
                    cfg.AllowAnyOrigin(); //对应跨域请求的地址
                    cfg.AllowAnyMethod(); //对应请求方法的Method
                    cfg.AllowAnyHeader(); //对应请求方法的Headers
                    //cfg.AllowCredentials(); //对应请求的withCredentials 值
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //静态图片访问
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(@"D:\GitHubDesktop\WebApiTest\WebApiTest\Images"),
                RequestPath = "/Images"
            });
        }
    }
}
