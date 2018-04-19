using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Models.ViewModel;
using VKM.Admin.Models.ViewModel.History;
using VKM.Admin.Providers;
using VKM.Admin.Services;

namespace VKM.Admin.Controllers
{
    [Authorize]
    [Route("api/v1/history")]
    public class HistoryController : Controller
    {
        private readonly Config config;
        private readonly HistoryService historyService;
        
        public HistoryController(IOptions<Config> config)
        {
            this.config = config.Value;
            var databaseProvider = new SqLiteDatabaseProvider(this.config.DatabaseConnectionString);
            historyService = new HistoryService(databaseProvider);
        }
        
        [HttpPost]
        [Route("")]
        public IActionResult AddHistoryItem([FromBody]HistoryItemViewModel vm)
        {
            var studentId = int.Parse(User.Claims.Single(c => c.Type == "sid").Value);

            historyService.AddHistoryItem(vm, studentId);

            return Ok();
        }
    }
}