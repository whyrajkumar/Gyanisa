using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models;
using Gyanisa.Services;
using Gyanisa.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Gyanisa.Data.Interface;

namespace Gyanisa.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    public class CoursesController : Controller
    {
        protected readonly ICourseRepository _repository;
        protected readonly ApplicationDbContext _context;
        public CoursesController(ICourseRepository repository, ApplicationDbContext context)
        {
            this._repository = repository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //if (c == 0) return View("Empty");
            return View(await _repository.GetAllAsync());
            //return View(await _context.Subjects.ToListAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Course collection = await _repository.GetByIdAsync(id);
                return View(collection);
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, CourseName, CourseCode,Slug,MetaTitle,MetaKeywords,MetaDescription,Metaabstract")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

         

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Course collection)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _repository.Create(collection);
        //        _repository.SaveAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(collection);
        //}

        public async Task<IActionResult> Edit(int id)
        {
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
        public async Task<IActionResult> Edit(int id, Course collection)
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
                    //await _subjectRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!SubjectExists(collection.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var colection = await _repository.GetByIdAsync(id);
            _repository.Delete(colection);
            //await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
