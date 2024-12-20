using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.Admin
{
    public class City
    {
        [key]
        public int CityID { get; set; }
        [Display(Name ="City Name")]
        public string CityName { get; set; }
        [Display(Name = "City Code")]
        public string CityCode { get; set; }
        public string UrbanStatus { get; set; }

        public int DistrictID  { get; set; }
        

    }
}
