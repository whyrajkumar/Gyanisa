using Gyanisa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class TutorCourse : SeoEntity
    {
        public int UserID { get; set; }
        public UserInformation UserInformation { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

    }
}