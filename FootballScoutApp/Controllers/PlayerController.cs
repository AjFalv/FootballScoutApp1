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
using FootballScoutApp.Helpers;

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
        public async Task<IActionResult> Index(string position = null, string nationality = null, string sortOrder = null)
        {
            // Set default values to null if they are not provided
            position = position ?? "";
            nationality = nationality ?? "";

            // Set ViewBag values for the selected options
            ViewBag.SelectedPosition = position;
            ViewBag.SelectedNationality = nationality;

            // Get the role for "Player"
            var playerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Player");

            // Filter the users to include only those with the "Player" role
            var playerIds = await _context.UserRoles
                                          .Where(ur => ur.RoleId == playerRole.Id)
                                          .Select(ur => ur.UserId)
                                          .ToListAsync();

            var players = _context.UserProfiles.Where(up => playerIds.Contains(up.Id));

            // Apply Position Filter
            if (!string.IsNullOrEmpty(position))
            {
                players = players.Where(up => up.Position == position);
            }

            // Apply Nationality Filter
            if (!string.IsNullOrEmpty(nationality))
            {
                players = players.Where(up => up.Nationality == nationality);
            }

            // Convert the queryable players to a list
            var playerList = await players.ToListAsync();

            // Calculate Age for Each Player
            foreach (var player in playerList)
            {
                if (player.DateOfBirth.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - player.DateOfBirth.Value.Year;
                    if (player.DateOfBirth.Value.Date > today.AddYears(-age)) age--; // Adjust if the birthday hasn't occurred yet this year
                    player.Age = age; // Assuming you have an 'Age' property in the UserProfile model
                }
            }

            // Apply Age Sorting
            switch (sortOrder)
            {
                case "age_asc":
                    playerList = playerList.OrderBy(p => p.Age).ToList();
                    break;
                case "age_desc":
                    playerList = playerList.OrderByDescending(p => p.Age).ToList();
                    break;
                default:
                    playerList = playerList.OrderBy(p => p.Firstname).ToList(); // Default sorting by name
                    break;
            }




            // Handle Positions
            var hardcodedPositions = new List<string> { "Goalkeeper", "Defender", "Midfielder", "Forward" };

            // Retrieve distinct positions from the database and filter out any empty or null positions
            var distinctPositions = await _context.UserProfiles
                                                  .Where(p => !string.IsNullOrWhiteSpace(p.Position))
                                                  .Select(p => p.Position.Trim())  // Change: Trimming white spaces to ensure consistency
                                                  .Distinct()
                                                  .ToListAsync();

            // Combine hardcoded positions with distinct positions, ensuring no duplicates
            var combinedPositions = hardcodedPositions
                .Concat(distinctPositions.Except(hardcodedPositions))
                .Distinct()
                .ToList();

            // Debugging output to verify what is happening with Positions
            if (combinedPositions == null || !combinedPositions.Any())
            {
                Console.WriteLine("No positions found or combinedPositions is null.");
            }
            else
            {
                Console.WriteLine("Positions found: " + string.Join(", ", combinedPositions));
            }

            // Populate the ViewBag with the positions for the dropdown
            ViewBag.Positions = new SelectList(combinedPositions, position);

            // Handle Nationalities
            var allCountries = ListHelpers.GetCountryList();  // Change: Fetching all possible countries

            var distinctNationalities = await _context.UserProfiles
                                                      .Where(p => !string.IsNullOrWhiteSpace(p.Nationality))
                                                      .Select(p => p.Nationality.Trim())  // Change: Trimming white spaces to ensure consistency
                                                      .Distinct()
                                                      .ToListAsync();

            // Combine all possible countries with those currently used, ensuring no duplicates
            var combinedNationalities = allCountries
                .Concat(distinctNationalities.Except(allCountries))
                .Distinct()
                .ToList();

            if (combinedNationalities == null || !combinedNationalities.Any())
            {
                Console.WriteLine("No nationalities found or combinedNationalities is null.");
            }
            else
            {
                Console.WriteLine("Nationalities found: " + string.Join(", ", combinedNationalities));
            }

            // Populate the ViewBag with the nationalities for the dropdown
            ViewBag.Nationalities = new SelectList(combinedNationalities, nationality);

            // Maintain the selected sort order in the ViewBag
            ViewBag.CurrentSortOrder = sortOrder;

            return View(playerList);
        }


        // GET: Player/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Id == id);
            if (playerProfile == null)
            {
                return NotFound();
            }

            // Calculate age
            if (playerProfile.DateOfBirth.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - playerProfile.DateOfBirth.Value.Year;
                if (playerProfile.DateOfBirth.Value.Date > today.AddYears(-age)) age--; // Adjust if the birthday hasn't occurred yet this year
                ViewBag.Age = age;
            }

            return View(playerProfile);
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
