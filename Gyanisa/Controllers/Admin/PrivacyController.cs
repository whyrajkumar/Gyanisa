using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models.Admin;
using Microsoft.AspNetCore.Authorization;

namespace Gyanisa.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    public class PrivacyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrivacyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Privacy
        public async Task<IActionResult> Index()
        {
            return View(await _context.TermAndServices.ToListAsync());
        }

        // GET: Privacy/Details/5
       [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termAndService = await _context.TermAndServices
                .SingleOrDefaultAsync(m => m.Id == id);
            if (termAndService == null)
            {
                return NotFound();
            }

            return View(termAndService);
        }

        // GET: Privacy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Privacy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Aboutus,FAQ,Guidelines,PaymentRates,Licenses,TermsCondition,PrivacyPolicy")] TermAndService termAndService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(termAndService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(termAndService);
        }

        // GET: Privacy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termAndService = await _context.TermAndServices.SingleOrDefaultAsync(m => m.Id == id);
            if (termAndService == null)
            {
                return NotFound();
            }
            return View(termAndService);
        }

        // POST: Privacy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Aboutus,FAQ,Guidelines,PaymentRates,Licenses,TermsCondition,PrivacyPolicy")] TermAndService termAndService)
        {
            if (id != termAndService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termAndService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TermAndServiceExists(termAndService.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(termAndService);
        }

        // GET: Privacy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termAndService = await _context.TermAndServices
                .SingleOrDefaultAsync(m => m.Id == id);
            if (termAndService == null)
            {
                return NotFound();
            }

            return View(termAndService);
        }

        // POST: Privacy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termAndService = await _context.TermAndServices.SingleOrDefaultAsync(m => m.Id == id);
            _context.TermAndServices.Remove(termAndService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TermAndServiceExists(int id)
        {
            return _context.TermAndServices.Any(e => e.Id == id);
        }
    }
}
