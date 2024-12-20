using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.ViewModel
{
    public class TeacherInquiryViewModel
    {   
        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Subject Required")]
        public string Subject { get; set; }

        [Required(ErrorMessage ="Location or Zip Required")]
        public string Zip { get; set; }
        
        [Required(ErrorMessage = "Mobile Required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Mobile is not valid")]
        public string Mobile { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        
        public string Message { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }
    }
}
