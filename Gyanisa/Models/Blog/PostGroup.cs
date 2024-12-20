using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.Blog
{
    public class PostGroup:SeoEntity
    {
        [Key]
        public int PostGroupID { get; set; }
        [Display(Name ="Post Group Name")]
        public string PostGroupName { get; set; }
        [Display(Name = "Post Group Code")]
        public string PostGroupCode { get; set; }
        [Display(Name = "Post Group Description")]
        public string PostGroupDescription { get; set; }
        public string Image { get; set; }
        

    }
}
