using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.Admin
{
    public class District
    {
        [Key]
        public int DistrictID { get; set; }
        [Display(Name = "District Name")]
        public string DistrictName { get; set; }
        [Display(Name = "District Code")]
        public string DistrictCode { get; set; }

        public int RegionID  { get; set; }


    }
}
