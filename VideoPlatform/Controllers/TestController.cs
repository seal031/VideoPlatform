using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlatform.DB;
using VideoPlatform.DB.Entity;
using VideoPlatform.Models;

namespace VideoPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly MysqlDbContext _dbContext;
        public TestController(MysqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public string Index()
        {
            return "abc";
        }

        [HttpGet]
        [Route("GetUser")]
        public TestResponseModel GetUser(string foo)
        {
            TestResponseModel model = new TestResponseModel();
            model.userId = 10;
            model.userName = "seal";
            model.resultCode = "200";
            return model;
        }

        [HttpPost]
        [Route("AddUser")]
        public string AddUser(UserEntity user)
        {
            //UserEntity user = UserEntity.fromJson(foo);
            using (_dbContext)
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            return "ok";
        }
    }

}
