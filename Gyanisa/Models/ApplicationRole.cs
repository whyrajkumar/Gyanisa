using Microsoft.AspNetCore.Identity;
using System;

namespace Gyanisa.Models
{
    public class ApplicationRole: IdentityRole<int>
    {
        public ApplicationRole(string name): base(name)
        {

        }
        public ApplicationRole()
        { }
    }
}