using Gyanisa.Data;

using Gyanisa.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.ViewComponents
{
    public class TeacherInquiryViewComponent: ViewComponent
    {
        private ApplicationDbContext _context;
        public TeacherInquiryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int id)
        {
            TeacherInquiryViewModel model = new TeacherInquiryViewModel()
            {
                UserID=id
            };
           
            return View("default",model);
        }
    }
}
