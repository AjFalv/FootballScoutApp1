using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballScoutApp.Controllers
{
    [Authorize]
    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager; 
        }

        //List all the roles created by users
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(IdentityRole model)
        {
            // avoid duplicate role
            //if (!await _roleManager.RoleExistsAsync(model.Name))
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(model.Name));    
            //}

            //if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            //{
            //    _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            //}

            // Check for duplicate role
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                // Create the role
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (!result.Succeeded)
                {
                    // Handle errors (optional, but good practice)
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model); // Return the view with errors if creation fails
                }
            }
            else
            {
                ModelState.AddModelError("", "Role already exists.");
                return View(model); // Return the view if role already exists
            }


            return RedirectToAction("Index");
        }
    }
}
