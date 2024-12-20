using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models.Blog;
using Microsoft.AspNetCore.Authorization;

namespace Gyanisa.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    //[Route("[controller]/[action]")]
    public class PostGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int pid;

        public PostGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostGroup.ToListAsync());
        }

        // GET: PostGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Details Not Found");
            }

            var postGroup = await _context.PostGroup
                .SingleOrDefaultAsync(m => m.PostGroupID == id);
            if (postGroup == null)
            {
                throw new Exception("Details Not Found");
            }

            return View(postGroup);
        }

        // GET: PostGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostGroupID,PostGroupName,PostGroupCode,PostGroupDescription,Image,Slug,MetaTitle,MetaKeywords,MetaDescription,Metaabstract")] PostGroup postGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postGroup);
        }

        // GET: PostGroups/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postGroup = await _context.PostGroup.SingleOrDefaultAsync(m => m.PostGroupID == id);
            pid = postGroup.PostGroupID;
            if (postGroup == null)
            {
                return NotFound();
            }
            return View(postGroup);
        }

        // POST: PostGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int PostGroupID, [Bind("PostGroupID,PostGroupName,PostGroupCode,PostGroupDescription,Image,Slug,MetaTitle,MetaKeywords,MetaDescription,Metaabstract")] PostGroup postGroup)
        {
            if (PostGroupID != postGroup.PostGroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostGroupExists(postGroup.PostGroupID))
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
            return View(postGroup);
        }

        // GET: PostGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postGroup = await _context.PostGroup
                .SingleOrDefaultAsync(m => m.PostGroupID == id);
            if (postGroup == null)
            {
                return NotFound();
            }

            return View(postGroup);
        }

        // POST: PostGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postGroup = await _context.PostGroup.SingleOrDefaultAsync(m => m.PostGroupID == id);
            _context.PostGroup.Remove(postGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostGroupExists(int id)
        {
            return _context.PostGroup.Any(e => e.PostGroupID == id);
        }
    }
}
