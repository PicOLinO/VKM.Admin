using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VKM.Admin.Models;
using VKM.Admin.Models.Database;
using VKM.Admin.Models.ViewModel.Student;
using VKM.Admin.Models.ViewModel.Team;
using VKM.Admin.Providers;
using VKM.Admin.Services;

namespace VKM.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly Config config;
        private readonly StudentService studentService;
        private readonly TeamService teamService;
        private readonly HistoryService historyService;
        
        public HomeController(IConfiguration configuration, IOptions<Config> config)
        {
            this.config = config.Value;
            
            var databaseProvider = new SqLiteDatabaseProvider(this.config.DatabaseConnectionString);
            studentService = new StudentService(databaseProvider);
            teamService = new TeamService(databaseProvider);
            historyService = new HistoryService(databaseProvider);
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            //TODO: Нужно это переделать на что нибудь другое... Подумать...
            //TODO: Также использовать Domain и Dto объекты, а не просто Domain.
            var teamsAndStudents = teamService.GetAllTeamsWithStudents();
            
            return View(teamsAndStudents);
        }

        #region Student

        [HttpGet]
        [Route("api/v1/student")]
        public IActionResult Student(int id)
        {
            var student = studentService.GetStudentById(id);
            var studentHistory = historyService.LoadStudentHistory(id);

            var json = new {Student = student, History = studentHistory};

            return Json(json);
        }

        [HttpPost]
        [Route("api/v1/student")]
        public IActionResult CreateStudent([FromBody]StudentCreateViewModel vm)
        {
            var id = studentService.CreateStudent(vm);

            return Ok(id);
        }
        
        [HttpPut]
        [Route("api/v1/student")]
        public IActionResult UpdateStudent([FromBody]StudentUpdateViewModel vm)
        {
            studentService.UpdateStudent(vm);

            return Ok();
        }
        
        [HttpDelete]
        [Route("api/v1/student")]
        public IActionResult RemoveStudent(int id)
        {
            studentService.DeleteStudentById(id);
            
            return Ok();
        }

        #endregion

        #region Team

        [HttpGet]
        [Route("api/v1/team")]
        public IActionResult Team(int id)
        {
            var team = teamService.GetTeamById(id);

            return Json(team);
        }

        [HttpGet]
        [Route("api/v1/teams")]
        public IActionResult Teams()
        {
            var teams = teamService.GetAllTeams();
            
            return Json(teams);
        }

        [HttpPost]
        [Route("api/v1/team")]
        public IActionResult CreateTeam([FromBody]TeamCreateViewModel vm)
        {
            var id = teamService.CreateTeam(vm);

            return Ok(id);
        }
        
        [HttpPut]
        [Route("api/v1/team")]
        public IActionResult UpdateTeam([FromBody]TeamUpdateViewModel vm)
        {
            teamService.UpdateTeam(vm);

            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/team")]
        public IActionResult RemoveTeam(int id)
        {
            teamService.DeleteTeamById(id);

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