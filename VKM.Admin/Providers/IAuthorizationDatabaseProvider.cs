﻿namespace VKM.Admin.Providers
{
    public interface IAuthorizationDatabaseProvider
    {
        int Authorize(string userName, string password);
        void Register(string userName, string password, int studentId);
        void ResetPassword(string userName, string newPassword);
    }
}