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
    public class BoleteriaController : Controller
    {
        private readonly CRPassContext _context;

        public BoleteriaController(CRPassContext context)
        {
            _context = context;
        }

        // GET: Boleterias
        public async Task<IActionResult> Index()
        {
            var cRPassContext = _context.Boleteria.Include(b => b.CodEmpresaNavigation);
            return View(await cRPassContext.ToListAsync());
        }

        // GET: Boleterias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteria = await _context.Boleteria
                .Include(b => b.CodEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.CodBoleteria == id);
            if (boleteria == null)
            {
                return NotFound();
            }

            return View(boleteria);
        }

        // GET: Boleterias/Create
        public IActionResult Create()
        {
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre");
            return View();
        }

        // POST: Boleterias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodBoleteria,CodEmpresa,Descripcion,Costo")] Boleteria boleteria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boleteria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", boleteria.CodEmpresa);
            return View(boleteria);
        }

        // GET: Boleterias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteria = await _context.Boleteria.FindAsync(id);
            if (boleteria == null)
            {
                return NotFound();
            }
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", boleteria.CodEmpresa);
            return View(boleteria);
        }

        // POST: Boleterias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodBoleteria,CodEmpresa,Descripcion,Costo")] Boleteria boleteria)
        {
            if (id != boleteria.CodBoleteria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boleteria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoleteriaExists(boleteria.CodBoleteria))
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
            ViewData["CodEmpresa"] = new SelectList(_context.Empresa, "CodEmpresa", "Nombre", boleteria.CodEmpresa);
            return View(boleteria);
        }

        // GET: Boleterias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteria = await _context.Boleteria
                .Include(b => b.CodEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.CodBoleteria == id);
            if (boleteria == null)
            {
                return NotFound();
            }

            return View(boleteria);
        }

        // POST: Boleterias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boleteria = await _context.Boleteria.FindAsync(id);
            _context.Boleteria.Remove(boleteria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoleteriaExists(int id)
        {
            return _context.Boleteria.Any(e => e.CodBoleteria == id);
        }
    }
}
