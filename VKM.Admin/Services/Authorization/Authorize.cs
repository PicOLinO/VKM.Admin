using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VKM.Admin.Services.Authorization
{
    public class Authorize : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            throw new NotImplementedException();
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            throw new NotImplementedException();
        }
    }
}