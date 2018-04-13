using Microsoft.AspNetCore.Mvc;
using VKM.Admin.Services;
using VKM.Admin.Services.Authorization;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Controllers
{
    [Authorize]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        
        public AuthController()
        {
            authorizationService = new AuthorizationService();
        }
        
        [HttpPost]
        [Route("login")]
        public IActionResult LoginStudent(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterStudent(string userName, string password, string confirmPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}