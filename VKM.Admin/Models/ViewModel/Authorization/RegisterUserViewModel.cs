using System.Net;

namespace VKM.Admin.Models.ViewModel.Authorization
{
    public class RegisterUserViewModel
    {
        public NetworkCredential Credential { get; set; }
        public int StudentId { get; set; }
    }
}