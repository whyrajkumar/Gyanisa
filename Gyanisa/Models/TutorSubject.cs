using Gyanisa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class TutorSubject : SeoEntity
    {
        public int UserID { get; set; }
        public UserInformation UserInformation { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
