using System.ComponentModel.DataAnnotations;

namespace RVCoreBoard.MVC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "사용자 ID를 입력하세요.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "사용자 ID를 입력하세요.")]
        public string Password { get; set; }
    }
}
