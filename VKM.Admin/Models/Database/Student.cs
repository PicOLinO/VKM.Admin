using System.Collections.Generic;

namespace VKM.Admin.Models.Database
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Group { get; set; }
        public Team Team { get; set; }
        public double AverageValue { get; set; }
        public List<HistoryItem> History { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";
    }
}