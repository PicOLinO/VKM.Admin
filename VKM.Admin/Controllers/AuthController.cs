using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VKM.Admin.Services;
using VKM.Admin.Services.Authorization;
using IAuthorizationService = VKM.Admin.Services.Interfaces.IAuthorizationService;

namespace VKM.Admin.Controllers
{
    [Authorize]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        
        public AuthController(IConfiguration configuration)
        {
            authorizationService = new AuthorizationService(configuration["Issuer"], configuration["Audience"], configuration["SigningKey"]);
        }
        
        [HttpPost]
        [Route("token")]
        public IActionResult LoginStudent(string userName, string password)
        {
            return Ok(new {token = authorizationService.Authorize(userName, password)});
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterStudent(string userName, string password, string confirmPassword, int studentId)
        {
            authorizationService.Register(userName, password, confirmPassword, studentId);

            return Ok();
        }
    }
}