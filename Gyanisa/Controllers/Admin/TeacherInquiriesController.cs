using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models;

namespace Gyanisa.Controllers.Admin
{
    public class TeacherInquiriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherInquiriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeacherInquiries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeacherInquiries.Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeacherInquiries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherInquiry = await _context.TeacherInquiries
                .Include(t => t.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teacherInquiry == null)
            {
                return NotFound();
            }

            return View(teacherInquiry);
        }

        // GET: TeacherInquiries/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: TeacherInquiries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Message,UserID,Id,FullName,Subject,Zip,Mobile,Email,IP,date,Archive,Complete")] TeacherInquiry teacherInquiry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherInquiry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", teacherInquiry.UserID);
            return View(teacherInquiry);
        }

        // GET: TeacherInquiries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherInquiry = await _context.TeacherInquiries.SingleOrDefaultAsync(m => m.Id == id);
            if (teacherInquiry == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", teacherInquiry.UserID);
            return View(teacherInquiry);
        }

        // POST: TeacherInquiries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Message,UserID,Id,FullName,Subject,Zip,Mobile,Email,IP,date,Archive,Complete")] TeacherInquiry teacherInquiry)
        {
            if (id != teacherInquiry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherInquiry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherInquiryExists(teacherInquiry.Id))
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
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", teacherInquiry.UserID);
            return View(teacherInquiry);
        }

        // GET: TeacherInquiries/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherInquiry = await _context.TeacherInquiries
                .Include(t => t.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teacherInquiry == null)
            {
                return NotFound();
            }

            return View(teacherInquiry);
        }

        // POST: TeacherInquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var teacherInquiry = await _context.TeacherInquiries.SingleOrDefaultAsync(m => m.Id == id);
            _context.TeacherInquiries.Remove(teacherInquiry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherInquiryExists(long id)
        {
            return _context.TeacherInquiries.Any(e => e.Id == id);
        }
    }
}
