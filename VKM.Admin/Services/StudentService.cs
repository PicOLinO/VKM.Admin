using System;
using VKM.Admin.Models.Database;
using VKM.Admin.Models.Database.Domain;
using VKM.Admin.Models.ViewModel.Student;
using VKM.Admin.Providers;

namespace VKM.Admin.Services
{
    public class StudentService : DatabaseService
    {
        public StudentService(IDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public Student GetStudentById(int id)
        {
            return DatabaseProvider.LoadStudentById(id);
        }

        public int CreateStudent(StudentCreateViewModel vm)
        {
            var id = DatabaseProvider.CreateStudent(vm.FirstName, vm.LastName, vm.MiddleName, vm.Group, vm.TeamId);
            return id;
        }

        public void UpdateStudent(StudentUpdateViewModel vm)
        {
            DatabaseProvider.UpdateStudent(vm.Id, vm.FirstName, vm.LastName, vm.MiddleName, vm.Group, vm.TeamId);
        }

        public void DeleteStudentById(int id)
        {
            DatabaseProvider.RemoveStudentById(id);
        }
    }
}