using Microsoft.AspNetCore.Mvc;
using VKM.Admin.Services;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        
        public AuthController()
        {
            authorizationService = new AuthorizationService();
        }
        
        public IActionResult LoginStudent(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult RegisterStudent(string userName, string password, string confirmPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}