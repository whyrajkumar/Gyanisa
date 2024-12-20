using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class Subject : SeoEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Subject Name")]
        public string SubjectName { get; set; }
        [Required]
        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; }

     
        public int SubjectCategoryId { get; set; }
        public SubjectCategory SubjectCategory { get; set; }

        public List<TutorSubject> TutorSubjects { get; set; }
    }
}
