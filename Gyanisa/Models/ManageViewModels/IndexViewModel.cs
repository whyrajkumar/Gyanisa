﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }
        //public string Firstname { get; set; }
        //public string Lastname { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
