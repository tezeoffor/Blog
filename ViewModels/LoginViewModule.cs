using System.ComponentModel.DataAnnotations;

namespace BlogApplication.ViewModels
{
    public class LoginViewModule
    {
        public string UserName { get; set; }

         [DataType(DataType.Password)]
         public string Password { get; set; }
    }
}
