using Gyanisa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.ViewComponents
{
    public class TutorRatingViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;
        public TutorRatingViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task <IViewComponentResult> InvokeAsync()
        {
            return View(await _context.SubjectCategories.ToListAsync());
        }
    }
}
