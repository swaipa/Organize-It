﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Organize_It.Data;
using Organize_It.Models;

namespace Organize_It.Controllers
{
    public class TodoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Todoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.todos.ToListAsync());
        }

        private IQueryable<Todo> GetMyToDoes()
        {
            IdentityUser currentUser = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();// Nichlas: Get the currrent user
            return _context.todos.Where(x => x.User == currentUser);
        }
        public ActionResult BuildToDoTable()
        {
            IdentityUser currentUser = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();// Nichlas: Get the currrent user

            return PartialView(
                "_ToDoTable", GetMyToDoes() /*_context.toDos.Where(x => x.User == currentUser)*/); // Nichlas: Only display the ToDoes made by the current user
        }

        // GET: Todoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Header,Description,IsDoing,IsDone")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                // Nichlas: Get the currrent user
                IdentityUser currentUser = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                if (currentUser != null)
                {
                    // Nichlas: Get the currrent users ID
                    string currentUserId = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;

                    // make a reference to the current user in the ToDo model 
                    todo.User = currentUser; // Nichlas
                }

                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Header,Description,IsDoing,IsDone")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
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
            return View(todo);
        }

        // GET: Todoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _context.todos.FindAsync(id);
            _context.todos.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(int id)
        {
            return _context.todos.Any(e => e.Id == id);
        }
    }
}
