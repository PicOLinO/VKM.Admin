namespace VKM.Admin.Models.ViewModel.Authorization
{
    public class LoginResponseViewModel
    {
        public string token { get; set; }
        public Database.Domain.Student Student { get; set; }
    }
}