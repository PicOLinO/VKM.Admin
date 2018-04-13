using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Models.Database;
using VKM.Admin.Services;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly Config config;
        private readonly IDatabaseProvider databaseProvider;
        private readonly IAuthorizationService authorizationService;
        
        public HomeController(IOptions<Config> config)
        {
            this.config = config.Value;
            databaseProvider = new SqLiteDatabaseProvider(this.config.DatabaseConnectionString);
            authorizationService = new AuthorizationService();
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var teams = databaseProvider.LoadTeamsAndStudents();
            
            return View(teams);
        }

        #region Student

        [HttpGet]
        [Route("api/v1/student")]
        public IActionResult Student(int id)
        {
            var student = databaseProvider.LoadStudentById(id);
            var studentHistory = databaseProvider.LoadHistoryByStudentId(id);

            var json = new {Student = student, History = studentHistory};

            return Json(json);
        }

        [HttpPost]
        [Route("api/v1/student")]
        public IActionResult CreateStudent(Student student)
        {
            var id = databaseProvider.CreateStudent(student);

            return Ok(id);
        }
        
        [HttpPut]
        [Route("api/v1/student")]
        public IActionResult UpdateStudent(Student student)
        {
            databaseProvider.UpdateStudent(student);

            return Ok();
        }
        
        [HttpDelete]
        [Route("api/v1/student")]
        public IActionResult RemoveStudent(int id)
        {
            databaseProvider.RemoveStudentById(id);

            return Ok();
        }

        #endregion

        #region Team

        [HttpGet]
        [Route("api/v1/team")]
        public IActionResult Team(int id)
        {
            var team = databaseProvider.LoadTeam(id);

            return Json(team);
        }

        [HttpGet]
        [Route("api/v1/teams")]
        public IActionResult Teams()
        {
            var teams = databaseProvider.LoadTeams();
            
            return Json(teams);
        }

        [HttpPost]
        [Route("api/v1/team")]
        public IActionResult CreateTeam(Team team)
        {
            var id = databaseProvider.CreateTeam(team);

            return Ok(id);
        }
        
        [HttpPut]
        [Route("api/v1/team")]
        public IActionResult UpdateTeam(Team team)
        {
            databaseProvider.UpdateTeam(team);

            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/team")]
        public IActionResult RemoveTeam(int id)
        {
            databaseProvider.RemoveTeamById(id);

            return Ok();
        }

        #endregion
        
        [Route("api/v1/error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}