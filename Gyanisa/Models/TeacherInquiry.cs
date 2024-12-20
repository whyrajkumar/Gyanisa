using Gyanisa.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class TeacherInquiry : PostRequire
    {
        public string Message { get; set; }

        public int UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
