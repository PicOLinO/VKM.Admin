using System.Collections.Generic;
using VKM.Admin.Models.Database;
using VKM.Admin.Models.ViewModel;
using VKM.Admin.Models.ViewModel.History;
using VKM.Admin.Providers;

namespace VKM.Admin.Services
{
    public class HistoryService : DatabaseService
    {
        public HistoryService(IDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public IEnumerable<HistoryItem> LoadStudentHistory(int studentId)
        {
            var history = DatabaseProvider.LoadHistoryByStudentId(studentId);
            return history;
        }
        
        public void AddHistoryItem(HistoryItemViewModel vm)
        {
            DatabaseProvider.AddHistoryItem(vm.AlgorithmName, vm.Date, vm.Value, vm.StudentId);
        }
    }
}