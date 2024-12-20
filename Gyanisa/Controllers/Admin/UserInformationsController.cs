using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gyanisa.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    public class UserInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserInformationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserInformations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserInformations.Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformations
                .Include(u => u.User)
                .SingleOrDefaultAsync(m => m.UserID == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            return View(userInformation);
        }

        // GET: UserInformations/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: UserInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,AddedDate,ModifiedDate,IPAddress,FirstName,LastName,Email,DOB,MobilePhone,Heading,HourlyFee,Add1,Add2,ZipCode,UserPhoto,Active,Gender,Roles,CvDescription,IsHisHome,IsYourHome,IsWebCam,GEducation,PEducation,Experience,ViewCount,Votes,IsMobile,IsEmail,IsAddress,IsEducation,IsGirl,IsBoy,IsCoed,Slug,MetaTitle,MetaKeywords,MetaDescription,Metaabstract")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", userInformation.UserID);
            return View(userInformation);
        }

        // GET: UserInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformations.SingleOrDefaultAsync(m => m.UserID == id);
            if (userInformation == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", userInformation.UserID);
            return View(userInformation);
        }

        // POST: UserInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,AddedDate,ModifiedDate,IPAddress,FirstName,LastName,Email,DOB,MobilePhone,Heading,HourlyFee,Add1,Add2,ZipCode,UserPhoto,Active,Gender,Roles,CvDescription,IsHisHome,IsYourHome,IsWebCam,GEducation,PEducation,Experience,ViewCount,Votes,IsMobile,IsEmail,IsAddress,IsEducation,IsGirl,IsBoy,IsCoed,Slug,MetaTitle,MetaKeywords,MetaDescription,Metaabstract")] UserInformation userInformation)
        {
            if (id != userInformation.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInformationExists(userInformation.UserID))
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
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", userInformation.UserID);
            return View(userInformation);
        }

        // GET: UserInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformations
                .Include(u => u.User)
                .SingleOrDefaultAsync(m => m.UserID == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            return View(userInformation);
        }

        // POST: UserInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userInformation = await _context.UserInformations.SingleOrDefaultAsync(m => m.UserID == id);
            _context.UserInformations.Remove(userInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInformationExists(int id)
        {
            return _context.UserInformations.Any(e => e.UserID == id);
        }
    }
}
