using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Models.Database;
using VKM.Admin.Services;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Controllers
{
    [Authorize]
    [Route("api/v1/history")]
    public class HistoryController : Controller
    {
        private readonly Config config;
        private readonly IDatabaseProvider databaseProvider;
        
        public HistoryController(IOptions<Config> config)
        {
            this.config = config.Value;
            databaseProvider = new SqLiteDatabaseProvider(this.config.DatabaseConnectionString);
        }
        
        [HttpPost]
        [Route("")]
        public IActionResult AddHistoryItem(HistoryItem historItem, int studentId)
        {
            databaseProvider.AddHistoryItem(historItem, studentId);

            return Ok();
        }
    }
}