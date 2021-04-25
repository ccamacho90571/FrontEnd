using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Models;

namespace FrontEnd.Controllers
{
    public class ControlAforoesController : Controller
    {
        private readonly CRPassContext _context;

        public ControlAforoesController(CRPassContext context)
        {
            _context = context;
        }

        // GET: ControlAforoes
        public async Task<IActionResult> Index()
        {
            var cRPassContext = _context.ControlAforo.Include(c => c.CodEmpresaNavigation);
            return View(await cRPassContext.ToListAsync());
        }

        // GET: ControlAforoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controlAforo = await _context.ControlAforo
                .Include(c => c.CodEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.CodControl == id);
            if (controlAforo == null)
            {
                return NotFound();
            }

            return View(controlAforo);
        }

        // GET: ControlAforoes/Create
        public IActionResult Create()
        {
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre");
            return View();
        }

        // POST: ControlAforoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodControl,CodEmpresa,NumeroDia,NumeroAforo")] ControlAforo controlAforo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controlAforo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", controlAforo.CodEmpresa);
            return View(controlAforo);
        }

        // GET: ControlAforoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controlAforo = await _context.ControlAforo.FindAsync(id);
            if (controlAforo == null)
            {
                return NotFound();
            }
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", controlAforo.CodEmpresa);
            return View(controlAforo);
        }

        // POST: ControlAforoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodControl,CodEmpresa,NumeroDia,NumeroAforo")] ControlAforo controlAforo)
        {
            if (id != controlAforo.CodControl)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controlAforo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControlAforoExists(controlAforo.CodControl))
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
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", controlAforo.CodEmpresa);
            return View(controlAforo);
        }

        // GET: ControlAforoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controlAforo = await _context.ControlAforo
                .Include(c => c.CodEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.CodControl == id);
            if (controlAforo == null)
            {
                return NotFound();
            }

            return View(controlAforo);
        }

        // POST: ControlAforoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controlAforo = await _context.ControlAforo.FindAsync(id);
            _context.ControlAforo.Remove(controlAforo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControlAforoExists(int id)
        {
            return _context.ControlAforo.Any(e => e.CodControl == id);
        }
    }
}
