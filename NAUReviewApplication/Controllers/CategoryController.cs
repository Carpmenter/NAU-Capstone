using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NAUReviewApplication.Models;

namespace NAUReviewApplication.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NAUcountryContext _context;

        public CategoryController(NAUcountryContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }


        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name")] Category @category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@category);
        }

        /**
        public async Task<IActionResult> getQuestionByCategory(int catID)
        {

            return View(@category);
        }**/

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @category = await _context.Category
                .SingleOrDefaultAsync(m => m.CategoryId == id);
            if (@category == null)
            {
                return NotFound();
            }

            return View(@category);
        }


        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.CategoryId == id);
            if (@category == null)
            {
                return NotFound();
            }
            return View(@category);
        }

        // POST: Category/Edit/5.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name")] Category @category)
        {
            if (id != @category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExists(@category.CategoryId))
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
            return View(@category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @category = await _context.Category
                .SingleOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(@category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @category = await _context.Category.SingleOrDefaultAsync(m => m.CategoryId == id);
            _context.Category.Remove(@category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CatExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}