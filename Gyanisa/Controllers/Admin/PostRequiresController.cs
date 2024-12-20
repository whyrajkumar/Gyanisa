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
    public class PostRequiresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostRequiresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostRequires
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostRequires.ToListAsync());
        }

        // GET: PostRequires/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                throw new Exception("Details Not Found");
            }

            var postRequire = await _context.PostRequires
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postRequire == null)
            {
                throw new Exception("Details Not Found");
            }

            return View(postRequire);
        }

        // GET: PostRequires/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostRequires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Subject,Zip,Mobile,Email,IP,date,Archive,Complete")] PostRequire postRequire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postRequire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postRequire);
        }

        // GET: PostRequires/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postRequire = await _context.PostRequires.SingleOrDefaultAsync(m => m.Id == id);
            if (postRequire == null)
            {
                return NotFound();
            }
            return View(postRequire);
        }

        // POST: PostRequires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FullName,Subject,Zip,Mobile,Email,IP,date,Archive,Complete")] PostRequire postRequire)
        {
            if (id != postRequire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postRequire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostRequireExists(postRequire.Id))
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
            return View(postRequire);
        }

        // GET: PostRequires/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postRequire = await _context.PostRequires
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postRequire == null)
            {
                return NotFound();
            }

            return View(postRequire);
        }

        // POST: PostRequires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var postRequire = await _context.PostRequires.SingleOrDefaultAsync(m => m.Id == id);
            _context.PostRequires.Remove(postRequire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostRequireExists(long id)
        {
            return _context.PostRequires.Any(e => e.Id == id);
        }
    }
}
