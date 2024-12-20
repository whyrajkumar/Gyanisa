using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.Blog
{
    public class Post: SeoEntity
    {
        public int Id { get; set; } 
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string Image { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;

       public int PostGroupID { get; set; }
        public PostGroup postGroup { get; set; }
    }
}
