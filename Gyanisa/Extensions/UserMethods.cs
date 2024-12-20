using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gyanisa.Extensions
{
    public static class UserMethods
    {
        //-------------< Class: ExtensionMethods >-------------

        public static string getUserId(this ClaimsPrincipal user)

        {

            //------------< getUserId(User) >------------

            //*returns the current UserID

            if (!user.Identity.IsAuthenticated)

                return null;



            ClaimsPrincipal currentUser = user;



            //< output >

            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            //</ output >

            //------------</ getUserId(User) >------------

        }

        //-------------</ Class: ExtensionMethods >-------------
    }
}
