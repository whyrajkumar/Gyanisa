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
using Gyanisa.Services;
using Gyanisa.Data.Interface;
using System.Linq.Expressions;
using Gyanisa.Models.ViewModel;

namespace Gyanisa.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    public class SubjectsController : Controller
    {
        private readonly ISubjectRepository _repository;
        private readonly ApplicationDbContext _context;
        public SubjectsController(ISubjectRepository repository, ApplicationDbContext context)
        {
           this._repository = repository;
            this._context = context;
        }
                
        public async Task<IActionResult> Index()
        {
            //if (c == 0) return View("Empty");

            //var sublist = (from s in _context.Subjects.Take(8)
            //               select new
            //               {
            //                   s.Id,
            //                   s.SubjectName,
            //                   s.SubjectCode,
            //                   s.SubjectCategoryId,
            //                   s.SubjectCategory.SubjectCategoryName,
            //                   s.SubjectCategory.SubjectCategoryCode,
            //                   s.SubjectCategory.SubjectCategoryDescription
            //               }).ToList();
            //return await _context.Subjects.ToListAsync();
           return View(await _context.Subjects.Include(s => s.SubjectCategory).ToListAsync());
         //   return View(await _context.Subjects.Include(s=>s.SubjectCategory.SubjectCategoryName).ToListAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Subject collection = await _repository.GetByIdAsync(id);
                return View(collection);
            }
        }
        public IActionResult Create()
        {
          
            ViewBag.SubjectCategorylist = _repository.GetAllWithSubjectCategory();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject collection)
        {
            if (ModelState.IsValid)
            {
                _repository.Create(collection);
              //  _context.Add(collection);
               await _repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
        }

     
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.SubjectCategorylist = _repository.GetAllWithSubjectCategory();
            var collection = await _repository.GetByIdAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject collection)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(collection);
                    await _repository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(collection.Id))
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
            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {  
            var collection = await _repository.GetByIdAsync(id);
            _repository.Delete(collection);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }


        //public async Task<bool> SubjectExists()
        //{
        //    var lista = _repository.FindAsync(i => i.Id == 1);
        //    return await false;// await lista;

        //}
        private bool SubjectExists(int id)
        {
            return false;
        }
    }
}
