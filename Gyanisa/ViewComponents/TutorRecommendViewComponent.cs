using Gyanisa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.ViewComponents
{
    public class TutorRecommendViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public TutorRecommendViewComponent (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        { 
            // var applicationDbContext = _context.UserDetails.Include(u => u.UserID);
            //var User = (from U in _context.UserInformations.Take(6)
            //           select new
            //           {
            //               U.FirstName,
            //               U.LastName,
            //               U.HourlyFee,
            //               U.Email,
            //               U.UserPhoto,
            //               U.Heading,
            //               U.UserID,
            //               U.Title
            //           }).ToList();
            //var dd = User.ToList();
            var userlist = await _context.UserInformations.Take(6).ToListAsync();
            ViewBag.Userlist = userlist;

            return View(await _context.UserInformations.Take(6).ToListAsync());
        }

    }
}
