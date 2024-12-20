using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class PostRequire
    {   
        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string Zip { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }
        public DateTime date { get; set; }
        public bool Archive { get; set; }
        public bool Complete { get; set; }
    }
}
