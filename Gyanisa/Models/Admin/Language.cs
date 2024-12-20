using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.Admin
{
    public class Language : SeoEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Language Name")]
        public string LanguageName { get; set; }

        [Display(Name = "Language Code")]
        [Required]
        public string LanguageCode { get; set; }

        public List<TaughtLanguage> TaughtLanguage { get; set; }
    }
}
