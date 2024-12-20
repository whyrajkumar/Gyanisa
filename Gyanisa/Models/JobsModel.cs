using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class JobsModel : SeoEntity
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Job Name")]
        public string Name { get; set; }
        [Display(Name = "Job Code")]
        public string Code { get; set; }
        [Display(Name = "Job Description")]
        public string Description { get; set; }

    }
}
