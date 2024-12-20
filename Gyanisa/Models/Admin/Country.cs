using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.Admin
{
    public class Country
    {
        [key]
        public int CountryID { get; set; }
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        //public virtual ICollection<Region> Regions { get; set; }

    }
}
