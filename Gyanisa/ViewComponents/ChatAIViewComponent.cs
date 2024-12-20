using Gyanisa.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.ViewComponents
{
    public class ChatAIViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ChatAIViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
