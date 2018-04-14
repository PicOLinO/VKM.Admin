using System.Collections.Generic;

namespace VKM.Admin.Models.Database.Domain
{
    public class TeamWithParticipants : Team
    {
        public List<Student> Students { get; set; }
    }
}