using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballScoutApp.Data;
using FootballScoutApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace FootballScoutApp.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly ApplicationDBContext _context;

        public PlayerController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Player
        public async Task<IActionResult> Index()
        {
              return _context.PlayerList != null ? 
                          View(await _context.PlayerList.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.PlayerList'  is null.");
        }

        // GET: Player/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PlayerList == null)
            {
                return NotFound();
            }

            var playerListEntity = await _context.PlayerList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playerListEntity == null)
            {
                return NotFound();
            }

            return View(playerListEntity);
        }

        // GET: Player/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Player/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Position")] PlayerListEntity playerListEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playerListEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(playerListEntity);
        }

        // GET: Player/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlayerList == null)
            {
                return NotFound();
            }

            var playerListEntity = await _context.PlayerList.FindAsync(id);
            if (playerListEntity == null)
            {
                return NotFound();
            }
            return View(playerListEntity);
        }

        // POST: Player/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Position")] PlayerListEntity playerListEntity)
        {
            if (id != playerListEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playerListEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerListEntityExists(playerListEntity.Id))
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
            return View(playerListEntity);
        }

        // GET: Player/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlayerList == null)
            {
                return NotFound();
            }

            var playerListEntity = await _context.PlayerList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playerListEntity == null)
            {
                return NotFound();
            }

            return View(playerListEntity);
        }

        // POST: Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlayerList == null)
            {
                return Problem("Entity set 'ApplicationDBContext.PlayerList'  is null.");
            }
            var playerListEntity = await _context.PlayerList.FindAsync(id);
            if (playerListEntity != null)
            {
                _context.PlayerList.Remove(playerListEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerListEntityExists(int id)
        {
          return (_context.PlayerList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
