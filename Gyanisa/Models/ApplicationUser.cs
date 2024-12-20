using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Gyanisa.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>    
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        
        public virtual UserInformation UserInformation { get; set; }
    }
}
