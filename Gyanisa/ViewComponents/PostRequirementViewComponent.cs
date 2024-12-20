using Gyanisa.Data;
using Gyanisa.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.ViewComponents
{
    public class PostRequirementViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public PostRequirementViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(PostRequireModel model)
        {
            return View("Default",model);
        }
       
    }
}
