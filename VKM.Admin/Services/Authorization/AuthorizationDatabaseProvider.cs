using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Services.Authorization
{
    public class AuthorizationDatabaseProvider : IAuthorizationDatabaseProvider
    {
        public string Authorize(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        public void Register(string userName, string password, string confirmPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}