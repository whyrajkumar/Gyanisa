using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class SubjectCategory : SeoEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Subject Category")]
        public string SubjectCategoryName { get; set; } // Music,Computer sciences
        [Required]
        [Display(Name = "Category Code")]
        public string SubjectCategoryCode { get; set; }

        [Display(Name = "Category Description")]
        public string SubjectCategoryDescription { get; set; }

        //public virtual List<Subject> Subjects { get; set; }
        public List<TutorSubjectCategory> TutorSubjectCategories { get; set; }

    }
}
