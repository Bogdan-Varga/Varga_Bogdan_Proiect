using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopModel.Data;
using ShopModel.Models;


namespace Varga_Bogdan_Proiect.Controllers
{
    public class CostumeController : Controller
    {
        private readonly Shop _context;

        public CostumeController(Shop context)
        {
            _context = context;
        }

        // GET: Costume
        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString,int? pageNumber)

        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DenumireSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Denumire_desc" : "";
            ViewData["PretSortParm"] = sortOrder == "Pret" ? "pret_desc" : "Pret";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var costume = from b in _context.Costume
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                costume = costume.Where(s => s.Denumire.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Denumire_desc":
                    costume = costume.OrderByDescending(b => b.Denumire);
                    break;
                case "Pret":
                    costume = costume.OrderBy(b => b.Pret);
                    break;
                case "pret_desc":
                    costume = costume.OrderByDescending(b => b.Pret);
                    break;
                default:
                    costume = costume.OrderBy(b => b.Denumire);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Costum>.CreateAsync(costume.AsNoTracking(), pageNumber ??
           1, pageSize));
        }

        // GET: Costume/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Costum = await _context.Costume
  .Include(s => s.Orders)
  .ThenInclude(e => e.Customer)
  .AsNoTracking()
  .FirstOrDefaultAsync(m => m.ID == id);
            if (Costum == null)
            {
                return NotFound();
            }

            return View(Costum);
        }

        // GET: Costume/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Costume/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Denumire,Categorie,Pret")] Costum costum)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(costum);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
        
            return View(costum);
        }

        // GET: Costume/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costum = await _context.Costume.FindAsync(id);
            if (costum == null)
            {
                return NotFound();
            }
            return View(costum);
        }

        // POST: Costume/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Costume.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Costum>(
            studentToUpdate,
            "",
            s => s.Categorie, s => s.Denumire, s => s.Pret))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Costume/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costum = await _context.Costume
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (costum == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(costum);
        }

        // POST: Costume/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var costum = await _context.Costume.FindAsync(id);
            if (costum == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Costume.Remove(costum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool CostumExists(int id)
        {
            return _context.Costume.Any(e => e.ID == id);
        }
    }
}
