using System.ComponentModel.DataAnnotations;

namespace SportsStore.ViewModels
{
  public class LoginViewModel
  {
    [Required]
    public string Name { get; set; }
    [Required]
    public string Password { get; set; }
  }
}
