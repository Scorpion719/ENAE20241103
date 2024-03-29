﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maestrodetalles.Models;

namespace Maestrodetalles.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly ENAE20241103DBContext _context;

        public ProyectoController(ENAE20241103DBContext context)
        {
            _context = context;
        }

        // GET: Proyecto
        public async Task<IActionResult> Index()
        {
              return _context.Proyectos != null ? 
                          View(await _context.Proyectos.ToListAsync()) :
                          Problem("Entity set 'ENAE20241103DBContext.Proyectos'  is null.");
        }

        // GET: Proyecto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proyectos == null)
            {
                return NotFound();
            }

            var proyecto = await _context.Proyectos
                .Include(s=>s.Tarea)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proyecto == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(proyecto);
        }

        // GET: Proyecto/Create
        public IActionResult Create()
        {
            var proyecto = new Proyecto();
            proyecto.FechaInicio = DateTime.Now;
            proyecto.FechaFin = DateTime.Now;
            proyecto.Tarea = new List<Tarea>();
            proyecto.Tarea.Add(new Tarea
            {
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now
            }) ;
            ViewBag.Accion = "Create";
            return View(proyecto);
        }

        // POST: Proyecto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,FechaInicio,FechaFin,Tarea")] Proyecto proyecto)
        {
            _context.Add(proyecto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //return View(proyecto);
        }
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("Id,Nombre,Descripcion,FechaInicio,FechaFin,Tarea")] Proyecto proyecto, string accion)
        {
            proyecto.Tarea.Add(new Tarea { FechaInicio = DateTime.Now, FechaFin = DateTime.Now });
            ViewBag.Accion = accion;
            return View(accion, proyecto);
        }
        public ActionResult EliminarDetalles([Bind("Id,Nombre,Descripcion,FechaInicio,FechaFin,Tarea")] Proyecto proyecto, int index, string accion)
        {
            var det = proyecto.Tarea[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                proyecto.Tarea.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, proyecto);
        }
        // GET: Proyecto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proyectos == null)
            {
                return NotFound();
            }

            var proyecto = await _context.Proyectos
                .Include(s => s.Tarea)
                .FirstAsync(s => s.Id == id);
            if (proyecto == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(proyecto);
        }

        // POST: Proyecto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,FechaInicio,FechaFin,Tarea")] Proyecto proyecto)
        {
            if (id != proyecto.Id)
            {
                return NotFound();
            }

            try
            {
                // Obtener los datos de la base de datos que van a ser modificados
                var facturaUpdate = await _context.Proyectos
                        .Include(s => s.Tarea)
                        .FirstAsync(s => s.Id == proyecto.Id);
                facturaUpdate.Nombre = proyecto.Nombre;
                facturaUpdate.Descripcion = proyecto.Descripcion;
                facturaUpdate.FechaInicio = proyecto.FechaInicio;
                facturaUpdate.FechaFin = proyecto.FechaFin;
                //facturaUpdate.Estado = proyecto.Estado;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = proyecto.Tarea.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    facturaUpdate.Tarea.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = proyecto.Tarea.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = facturaUpdate.Tarea.FirstOrDefault(s => s.Id == d.Id);
                    det.Nombre = d.Nombre;
                    det.Descripcion = d.Descripcion;
                    det.FechaInicio = d.FechaInicio;
                    det.FechaFin = d.FechaFin;
                    det.Estado = d.Estado;
                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDet = proyecto.Tarea.Where(s => s.Id < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.Tarea.FirstOrDefault(s => s.Id == d.Id);
                        _context.Remove(det);
                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos


                _context.Update(facturaUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProyectoExists(proyecto.Id))
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

        // GET: Proyecto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proyectos == null)
            {
                return NotFound();
            }

            var proyecto = await _context.Proyectos
                .Include(s => s.Tarea)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proyecto == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";

            return View(proyecto);
        }

        // POST: Proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proyectos == null)
            {
                return Problem("Entity set 'ENAE20241103DBContext.Proyectos'  is null.");
            }
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto != null)
            {
                _context.Proyectos.Remove(proyecto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProyectoExists(int id)
        {
          return (_context.Proyectos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
