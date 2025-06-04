using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using demo2.Data;
using demo2.Models;
using demo2.ViewModels; // Thêm using cho ViewModel
using System.IO;

namespace demo2.Controllers
{
    public class FlowersController : Controller
    {
        private readonly AppDbContext _context;

        public FlowersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var flowers = await _context.Flowers.Include(f => f.Category).ToListAsync();
            return View(flowers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var flower = await _context.Flowers
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flower == null) return NotFound();

            return View(flower);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlowerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(model.ImageFile.FileName);
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    imagePath = "/images/" + fileName;
                }

                var flower = new Flower
                {
                    Name = model.Name,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    ImageUrl = imagePath // SỬA Ở ĐÂY
                };

                _context.Flowers.Add(flower);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var flower = await _context.Flowers.FindAsync(id);
            if (flower == null) return NotFound();

            ViewBag.CategoryList = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View(flower);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Flower flower, IFormFile? ImageFile)
        {
            if (id != flower.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(ImageFile.FileName);
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        flower.ImageUrl = "/images/" + fileName; // SỬA Ở ĐÂY
                    }

                    _context.Update(flower);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlowerExists(flower.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View(flower);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var flower = await _context.Flowers
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flower == null) return NotFound();

            return View(flower);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flower = await _context.Flowers.FindAsync(id);
            if (flower != null)
            {
                _context.Flowers.Remove(flower);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FlowerExists(int id)
        {
            return _context.Flowers.Any(e => e.Id == id);
        }
    }
}
