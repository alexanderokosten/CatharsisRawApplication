using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CatharsisRawWebApplication.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
     
        public string UserName { get; set; }


        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Телефон")]
        [Phone]
        public string PhoneNumber { get; set; }



        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
