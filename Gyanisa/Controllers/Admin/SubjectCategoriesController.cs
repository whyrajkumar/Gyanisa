using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models;
using Gyanisa.Data.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Gyanisa.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    public class SubjectCategoriesController : Controller
    {
        private readonly ISubjectCategoryRepository _repository;
        public SubjectCategoriesController(ISubjectCategoryRepository repository)
        {
            this._repository = repository;
        }


        public async Task<IActionResult> Index()
        {
            //if (c == 0) return View("Empty");
            return View(await _repository.GetAllAsync());
            //return View(await _context.Subjects.ToListAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id != null)
            {
                SubjectCategory collection = await _repository.GetByIdAsync(id);
                return View(collection);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectCategory collection)
        {
            if (!ModelState.IsValid)
            {
                return View(collection);
            }
          _repository.Create(collection);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }


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
        public async Task<IActionResult> Edit(int id, SubjectCategory collection)
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
                    if (!SubjectCategoryExists(collection.Id))
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
        private bool SubjectCategoryExists(int id)
        {
            return false;
        }
    }
}
