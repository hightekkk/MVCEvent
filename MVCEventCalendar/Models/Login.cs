using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCEventCalendar.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Пользователь не найден", AllowEmptyStrings =false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Неправильный пароль", AllowEmptyStrings =false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}