using System;

namespace VKM.Admin.Models.ViewModel.History
{
    public class HistoryItemViewModel
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public string AlgorithmName { get; set; }
    }
}