using System.Collections.Generic;

namespace VKM.Admin.Models.Database
{
    public class Team
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<Student> Students { get; set; }
    }
}