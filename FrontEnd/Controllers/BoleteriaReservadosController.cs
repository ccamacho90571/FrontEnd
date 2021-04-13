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
    public class BoleteriaReservadosController : Controller
    {
        private readonly CRPassContext _context;

        public BoleteriaReservadosController(CRPassContext context)
        {
            _context = context;
        }

        // GET: BoleteriaReservados
        public async Task<IActionResult> Index()
        {
            var cRPassContext = _context.BoleteriaReservados.Include(b => b.CodBoleteriaNavigation).Include(b => b.CodTicketsNavigation);
            return View(await cRPassContext.ToListAsync());
        }

        // GET: BoleteriaReservados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteriaReservados = await _context.BoleteriaReservados
                .Include(b => b.CodBoleteriaNavigation)
                .Include(b => b.CodTicketsNavigation)
                .FirstOrDefaultAsync(m => m.CodBoletaReservado == id);
            if (boleteriaReservados == null)
            {
                return NotFound();
            }

            return View(boleteriaReservados);
        }

        // GET: BoleteriaReservados/Create
        public IActionResult Create()
        {
            ViewData["CodBoleteria"] = new SelectList(_context.Boleteria, "CodBoleteria", "Descripcion");
            ViewData["CodTickets"] = new SelectList(_context.Tickets, "CodTicket", "Nreserva");
            return View();
        }

        // POST: BoleteriaReservados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodBoletaReservado,CodBoleteria,CodTickets,Cantidad")] BoleteriaReservados boleteriaReservados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boleteriaReservados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodBoleteria"] = new SelectList(_context.Boleteria, "CodBoleteria", "Descripcion", boleteriaReservados.CodBoleteria);
            ViewData["CodTickets"] = new SelectList(_context.Tickets, "CodTicket", "Nreserva", boleteriaReservados.CodTickets);
            return View(boleteriaReservados);
        }

        // GET: BoleteriaReservados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteriaReservados = await _context.BoleteriaReservados.FindAsync(id);
            if (boleteriaReservados == null)
            {
                return NotFound();
            }
            ViewData["CodBoleteria"] = new SelectList(_context.Boleteria, "CodBoleteria", "Descripcion", boleteriaReservados.CodBoleteria);
            ViewData["CodTickets"] = new SelectList(_context.Tickets, "CodTicket", "Nreserva", boleteriaReservados.CodTickets);
            return View(boleteriaReservados);
        }

        // POST: BoleteriaReservados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodBoletaReservado,CodBoleteria,CodTickets,Cantidad")] BoleteriaReservados boleteriaReservados)
        {
            if (id != boleteriaReservados.CodBoletaReservado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boleteriaReservados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoleteriaReservadosExists(boleteriaReservados.CodBoletaReservado))
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
            ViewData["CodBoleteria"] = new SelectList(_context.Boleteria, "CodBoleteria", "Descripcion", boleteriaReservados.CodBoleteria);
            ViewData["CodTickets"] = new SelectList(_context.Tickets, "CodTicket", "Nreserva", boleteriaReservados.CodTickets);
            return View(boleteriaReservados);
        }

        // GET: BoleteriaReservados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteriaReservados = await _context.BoleteriaReservados
                .Include(b => b.CodBoleteriaNavigation)
                .Include(b => b.CodTicketsNavigation)
                .FirstOrDefaultAsync(m => m.CodBoletaReservado == id);
            if (boleteriaReservados == null)
            {
                return NotFound();
            }

            return View(boleteriaReservados);
        }

        // POST: BoleteriaReservados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boleteriaReservados = await _context.BoleteriaReservados.FindAsync(id);
            _context.BoleteriaReservados.Remove(boleteriaReservados);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoleteriaReservadosExists(int id)
        {
            return _context.BoleteriaReservados.Any(e => e.CodBoletaReservado == id);
        }
    }
}
