﻿using System.ComponentModel.DataAnnotations;

namespace ChessWeb.Application.ViewModels.User
{
    public class UserRegisterViewModel
    {
        [Required]
        [Display(Name = "Имя юзверя")]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль-король")]
        public string Password { get; set; }
 
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пересечение указанных паролей дает пустое множество")]
        [DataType(DataType.Password)]
        [Display(Name = "Еще раз пароль")]
        public string PasswordConfirm { get; set; }
    }
}