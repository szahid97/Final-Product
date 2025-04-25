using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;
using TaskTrackerDemo.Models;

namespace TaskTrackerDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: /Admin/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToListAsync();

            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            var managerOptions = managers.Select(m => new SelectListItem
            {
                Value = m.Id,
                Text = m.FullName
            }).ToList();

            var model = new RegisterViewModel
            {
                Roles = roles,
                Managers = managerOptions
            };

            return View(model);
        }



        // POST: /Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToListAsync();

                var managers = await _userManager.GetUsersInRoleAsync("Manager");
                model.Managers = managers.Select(m => new SelectListItem
                {
                    Value = m.Id,
                    Text = m.FullName
                }).ToList();

                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                IsManager = model.Role == "Manager",
                ManagerId = model.Role == "Member" ? model.ManagerId : null,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    if (model.Role == "Admin")
                    {
                        if (!await _userManager.IsInRoleAsync(user, "Manager"))
                        {
                            await _userManager.AddToRoleAsync(user, "Manager");
                        }

                         if (!user.IsManager)
                        {
                            user.IsManager = true;
                            await _userManager.UpdateAsync(user);
                        }
                    }

                }

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.Roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToListAsync();

            return View(model);
        }

        // GET: /Admin/Edit/id
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault();

            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Admin", Text = "Admin" },
                new SelectListItem { Value = "Manager", Text = "Manager" },
                new SelectListItem { Value = "Member", Text = "Member" }
            };

            var managers = await _userManager.Users
                .Where(u => u.IsManager)
                .Select(m => new SelectListItem
                {
                    Value = m.Id,
                    Text = m.FullName
                })
                .ToListAsync();

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = currentRole,
                Roles = roles,
                ManagerId = user.ManagerId,
                Managers = managers
            };

            return View(viewModel);
        }

        // POST: /Admin/Edit/id
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.ManagerId = model.Role == "Member" ? model.ManagerId : null;

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault();

            if (currentRole != model.Role)
            {
                if (!string.IsNullOrEmpty(currentRole))
                    await _userManager.RemoveFromRoleAsync(user, currentRole);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction("Index");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // POST: /Admin/Delete/id
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}
