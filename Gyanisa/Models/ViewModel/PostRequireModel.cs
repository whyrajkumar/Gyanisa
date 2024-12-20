using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.ViewModel
{
    public class PostRequireModel
    {   
        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        [DataType(DataType.PostalCode, ErrorMessage = "Zip is not valid")]
        public string Zip { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Mobile is not valid")]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
    }
}
