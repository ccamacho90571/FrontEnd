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
    public class PublicidadsController : Controller
    {
        private readonly CRPassContext _context;

        public PublicidadsController(CRPassContext context)
        {
            _context = context;
        }

        // GET: Publicidads
        public async Task<IActionResult> Index()
        {
            var cRPassContext = _context.Publicidad.Include(p => p.CodEmpresaNavigation);
            return View(await cRPassContext.ToListAsync());
        }

        // GET: Publicidads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicidad = await _context.Publicidad
                .Include(p => p.CodEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.CodPublicidad == id);
            if (publicidad == null)
            {
                return NotFound();
            }

            return View(publicidad);
        }

        // GET: Publicidads/Create
        public IActionResult Create()
        {
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre");
            return View();
        }

        // POST: Publicidads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodPublicidad,CodEmpresa,RutaArchivo")] Publicidad publicidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publicidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", publicidad.CodEmpresa);
            return View(publicidad);
        }

        // GET: Publicidads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicidad = await _context.Publicidad.FindAsync(id);
            if (publicidad == null)
            {
                return NotFound();
            }
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", publicidad.CodEmpresa);
            return View(publicidad);
        }

        // POST: Publicidads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodPublicidad,CodEmpresa,RutaArchivo")] Publicidad publicidad)
        {
            if (id != publicidad.CodPublicidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publicidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicidadExists(publicidad.CodPublicidad))
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
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", publicidad.CodEmpresa);
            return View(publicidad);
        }

        // GET: Publicidads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicidad = await _context.Publicidad
                .Include(p => p.CodEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.CodPublicidad == id);
            if (publicidad == null)
            {
                return NotFound();
            }

            return View(publicidad);
        }

        // POST: Publicidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publicidad = await _context.Publicidad.FindAsync(id);
            _context.Publicidad.Remove(publicidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicidadExists(int id)
        {
            return _context.Publicidad.Any(e => e.CodPublicidad == id);
        }
    }
}
