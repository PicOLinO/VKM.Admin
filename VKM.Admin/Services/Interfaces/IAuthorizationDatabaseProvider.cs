namespace VKM.Admin.Services.Interfaces
{
    public interface IAuthorizationDatabaseProvider
    {
        bool Authorize(string userName, string password);
        void Register(string userName, string password, int studentId);
        void ResetPassword(string userName, string newPassword);
    }
}