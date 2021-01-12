using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopModel.Data;
using ShopModel.Models;
using Varga_Bogdan_Proiect.Models.ShopViewModels;

namespace Varga_Bogdan_Proiect.Controllers
{
    public class MagazineController : Controller
    {
        private readonly Shop _context;

        public MagazineController(Shop context)
        {
            _context = context;
        }

        // GET: Magazine
        public async Task<IActionResult> Index(int? id, int? costumID)
        {
            var viewModel = new MagazinIndexData();
            viewModel.Magazine = await _context.Magazine
            .Include(i => i.MagazinCostume)
            .ThenInclude(i => i.Costum)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.MagazinName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["MagazinID"] = id.Value;
                Magazin magazin = viewModel.Magazine.Where(
                i => i.ID == id.Value).Single();
                viewModel.Costume = magazin.MagazinCostume.Select(s => s.Costum);
            }
            if (costumID != null)
            {
                ViewData["CostumID"] = costumID.Value;
                viewModel.Orders = viewModel.Costume.Where(
                x => x.ID == costumID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Magazine/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazin = await _context.Magazine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (magazin == null)
            {
                return NotFound();
            }

            return View(magazin);
        }

        // GET: Magazine/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Magazine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MagazinName,Adress")] Magazin magazin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(magazin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(magazin);
        }

        // GET: Magazine/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publisher = await _context.Magazine
            .Include(i => i.MagazinCostume).ThenInclude(i => i.Costum)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }
            PopulateMagazinCostumData(publisher);
            return View(publisher);

        }
        private void PopulateMagazinCostumData(Magazin magazin)
        {
            var allCostume = _context.Costume;
            var magazinCostume = new HashSet<int>(magazin.MagazinCostume.Select(c => c.CostumID));
            var viewModel = new List<MagazinCostumData>();
            foreach (var costum in allCostume)
            {
                viewModel.Add(new MagazinCostumData
                {
                    CostumID = costum.ID,
                    Denumire = costum.Denumire,
                    IsMagazin = magazinCostume.Contains(costum.ID)
                });
            }
            ViewData["Costume"] = viewModel;
        }

        // POST: Magazine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCostume)
        {
            if (id == null)
            {
                return NotFound();
            }
            var magazinToUpdate = await _context.Magazine
            .Include(i => i.MagazinCostume)
            .ThenInclude(i => i.Costum)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Magazin>(
            magazinToUpdate,
            "",
            i => i.MagazinName, i => i.Adress))
            {
                UpdateMagazinCostume(selectedCostume, magazinToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateMagazinCostume(selectedCostume, magazinToUpdate);
            PopulateMagazinCostumData(magazinToUpdate);
            return View(magazinToUpdate);
        }
        private void UpdateMagazinCostume(string[] selectedCostume, Magazin magazinToUpdate)
        {
            if (selectedCostume == null)
            {
                magazinToUpdate.MagazinCostume = new List<MagazinCostum>();
                return;
            }
            var selectedCostumeHS = new HashSet<string>(selectedCostume);
            var magazinCostume = new HashSet<int>
            (magazinToUpdate.MagazinCostume.Select(c => c.Costum.ID));
            foreach (var costum in _context.Costume)
            {
                if (selectedCostumeHS.Contains(costum.ID.ToString()))
                {
                    if (!magazinCostume.Contains(costum.ID))
                    {
                        magazinToUpdate.MagazinCostume.Add(new MagazinCostum
                        {
                            MagazinID =
                       magazinToUpdate.ID,
                            CostumID = costum.ID
                        });
                    }
                }
                else
                {
                    if (magazinCostume.Contains(costum.ID))
                    {
                        MagazinCostum costumToRemove = magazinToUpdate.MagazinCostume.FirstOrDefault(i
                       => i.CostumID == costum.ID);
                        _context.Remove(costumToRemove);
                    }
                }
            }
        }

        // GET: Magazine/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazin = await _context.Magazine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (magazin == null)
            {
                return NotFound();
            }

            return View(magazin);
        }

        // POST: Magazine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var magazin = await _context.Magazine.FindAsync(id);
            _context.Magazine.Remove(magazin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MagazinExists(int id)
        {
            return _context.Magazine.Any(e => e.ID == id);
        }
    }
}
