using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VKM.Admin.Models;
using VKM.Admin.Services;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabaseProvider databaseProvider;
        
        public HomeController()
        {
            databaseProvider = new XmlDatabaseProvider(); //TODO: Set xml database path in ctor of XmlDatabaseProvider.
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