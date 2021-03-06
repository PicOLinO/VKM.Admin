﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VKM.Admin.Models.ViewModel.Authorization;
using VKM.Admin.Providers;

namespace VKM.Admin.Services.Authorization
{
    public class AuthorizationService
    {
        private readonly IAuthorizationDatabaseProvider databaseProvider;
        private readonly IDatabaseProvider baseDatabaseProvider;

        private readonly string issuer;
        private readonly string audience;
        private readonly string signingKey;
        
        public AuthorizationService(string databaseConnectionString, string issuer, string audience, string signingKey)
        {
            this.issuer = issuer;
            this.audience = audience;
            this.signingKey = signingKey;
            
            databaseProvider = new AuthorizationDatabaseProvider(databaseConnectionString);
            baseDatabaseProvider = new SqLiteDatabaseProvider(databaseConnectionString);
        }
        
        public LoginResponseViewModel Authorize(string userName, string password)
        {
            var id = databaseProvider.Authorize(userName, password);
            if (id < 0)
            {
                throw new UnauthorizedAccessException();
            }
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Sid, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var student = baseDatabaseProvider.LoadStudentById(id);

            var result = new LoginResponseViewModel
                           {
                               Student = student,
                               token = tokenString
                           };
            return result;
        }

        public void Register(string userName, string password, int studentId)
        {
            databaseProvider.Register(userName, password, studentId);
        }

        public void ResetPassword(string userName, string newPassword)
        {
            databaseProvider.ResetPassword(userName, newPassword);
        }
    }
}