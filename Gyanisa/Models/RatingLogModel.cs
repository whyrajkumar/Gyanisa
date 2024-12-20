
using Gyanisa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class RatingLogModel : SeoEntity 
    { 
        [Key]
        public int AutoId { get; set; }
        public int SectionId { get; set; }
        public int VoteForID { get; set; }
        public bool Active { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public UserInformation UserInformation { get; set; }

    }
}
