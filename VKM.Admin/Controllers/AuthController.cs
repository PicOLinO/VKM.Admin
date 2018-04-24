using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Models.Database;
using VKM.Admin.Models.ViewModel;
using VKM.Admin.Models.ViewModel.Authorization;
using VKM.Admin.Services;
using VKM.Admin.Services.Authorization;

namespace VKM.Admin.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly Config config;
        private readonly AuthorizationService authorizationService;
        
        public AuthController(IConfiguration configuration, IOptions<Config> config)
        {
            this.config = config.Value;
            authorizationService = new AuthorizationService(this.config.DatabaseConnectionString, configuration["Issuer"], configuration["Audience"], configuration["SigningKey"]);
        }
        
        [HttpPost]
        [Route("token")]
        public IActionResult LoginStudent([FromBody]LoginViewModel vm)
        {
            var response = authorizationService.Authorize(vm.UserName, vm.Password);
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterStudent([FromBody]RegisterUserViewModel vm)
        {
            authorizationService.Register(vm.Credential.UserName, vm.Credential.Password, vm.StudentId);

            return Ok();
        }

        [HttpPost]
        [Route("resetpwd")]
        public IActionResult ResetPassword([FromBody]ResetPasswordViewModel vm)
        {
            authorizationService.ResetPassword(vm.UserName, vm.NewPassword);

            return Ok();
        }
    }
}