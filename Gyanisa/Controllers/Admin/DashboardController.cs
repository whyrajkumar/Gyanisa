using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyanisa.Data;
using Gyanisa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gyanisa.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public readonly ApplicationDbContext _context;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public DashboardController(ApplicationDbContext context,SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task <ActionResult> Index()
        {
           ViewData["PostReq"] = await _context.PostRequires.CountAsync();
           ViewData["TeacherReq"] = await _context.TeacherInquiries.CountAsync();
           return View();
        }
        
        public async Task<ActionResult> Dashboard()
        {

            return View(await _context.PostRequires.ToListAsync());
        }

        public ActionResult Details(int id)
        {
            return View();
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        public ActionResult Delete(int id)
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

         
        public ActionResult PostReq(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }



    }
}