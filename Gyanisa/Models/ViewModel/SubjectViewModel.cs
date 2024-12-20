using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.ViewModel
{
    public class SubjectViewModel
    {
            //public int Id { get; set; }
            //[Required]
            //[Display(Name = "Subject Name")]
            //public string SubjectName { get; set; }
            //[Required]
            //[Display(Name = "Subject Code")]
            //public string SubjectCode { get; set; }
            //public int SubjectCategoryId { get; set; }

        public Subject Subjectnew { get; set; }
        public IEnumerable<SubjectCategory> subjectCategoriesnew { get; set; }
        
    }
}
