using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ComplexProject.Models
{
    public class RegisterViewModel : Controller
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }


        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
