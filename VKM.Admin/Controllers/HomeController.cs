using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Services;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly Config config;
        private readonly IDatabaseProvider databaseProvider;
        
        public HomeController(IOptions<Config> config)
        {
            this.config = config.Value;
            databaseProvider = new XmlDatabaseProvider(this.config.DatabasePath);
        }
        
        public IActionResult Index()
        {
            var teams = databaseProvider.LoadAllTeams();
            
            return View(teams);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}