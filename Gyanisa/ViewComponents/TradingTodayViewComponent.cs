﻿using Gyanisa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.ViewComponents
{
    public class TradingTodayViewComponent: ViewComponent
    {
        private ApplicationDbContext _context;

        public TradingTodayViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.UserInformations.Take(3).ToListAsync());
        }
       
    }
}
