using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Models.Database;
using VKM.Admin.Models.ViewModel;
using VKM.Admin.Services;
using VKM.Admin.Services.Authorization;
using IAuthorizationService = VKM.Admin.Services.Interfaces.IAuthorizationService;

namespace VKM.Admin.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly Config config;
        private readonly IAuthorizationService authorizationService;
        
        public AuthController(IConfiguration configuration, IOptions<Config> config)
        {
            this.config = config.Value;
            authorizationService = new AuthorizationService(this.config.DatabaseConnectionString, configuration["Issuer"], configuration["Audience"], configuration["SigningKey"]);
        }
        
        [HttpPost]
        [Route("token")]
        public IActionResult LoginStudent([FromBody]User user)
        {
            return Ok(new {token = authorizationService.Authorize(user.Login, user.Password)});
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterStudent([FromBody]RegisterUserViewModel user)
        {
            authorizationService.Register(user.Login, user.Password, user.ConfirmPassword, user.StudentId);

            return Ok();
        }
    }
}