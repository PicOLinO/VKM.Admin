namespace VKM.Admin.Services.Interfaces
{
    public interface IAuthorizationService
    {
        string Authorize(string userName, string password);
        void Register(string userName, string password, string confirmPassword, int studentId);
        void ResetPassword(string userName, string newPassword);
    }
}