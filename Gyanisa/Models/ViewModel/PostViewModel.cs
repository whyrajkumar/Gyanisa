using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.ViewModel
{
    public class PostViewModel:SeoEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public IFormFile Image { get; set; } = null;
        public string CurrentImage { get; internal set; }

        public int Postgroupid { get; set; }
    }
}
