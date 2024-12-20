using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class SeoEntity
    {
        [Display(Name ="Slug Name")]
        public string Slug { get; set; }

        [Display(Name = "Meta Title")]
        public string MetaTitle { get; set; }
       
     //   [StringLength(150, ErrorMessage = "Use 5-6 Words")]
        [Display(Name = "Meta Keywords")]
        public string MetaKeywords { get; set; }

        [Display(Name = "Meta Description")]
        public string MetaDescription{ get; set; }

        [Display(Name = "Meta Abstract")]
        public string Metaabstract { get; set; }
    }
}
