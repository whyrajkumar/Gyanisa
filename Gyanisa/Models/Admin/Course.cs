using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class Course : SeoEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } 

        [Required]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        public List<TutorCourse> TutorCourses { get; set; }
    }
}
