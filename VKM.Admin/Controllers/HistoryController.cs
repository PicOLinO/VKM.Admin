using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Services;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Controllers
{
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
        [Route("api/v1/history")]
        public IActionResult AddHistoryItem()
        {
            throw new NotImplementedException();
        }
    }
}