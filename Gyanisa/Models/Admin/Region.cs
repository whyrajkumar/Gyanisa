using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.Admin
{
    public class Region
    {
        [key]
        public int RegionID { get; set; }
        [Display(Name ="Region Name")]
        public string RegionName { get; set; }
        [Display(Name = "Region Code")]
        public string RegionCode { get; set; }

        public int CountryID  { get; set; }
    }
}
