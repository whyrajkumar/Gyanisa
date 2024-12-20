using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class TutorSubjectCategory
    {
        public int UserID { get; set; }
        public UserInformation UserInformation { get; set; }

        public int SubjectCategoryId { get; set; }
        public SubjectCategory SubjectCategory { get; set; }
    }
}
