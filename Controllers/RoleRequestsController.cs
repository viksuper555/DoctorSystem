using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorSystem.Data;
using DoctorSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using DoctorSystem.Misc;

namespace DoctorSystem.Controllers
{
    public class RoleRequestsController : Controller
    {

        private readonly UserManager<DefaultUser> _userManager;

        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRequestsController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<DefaultUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: RoleRequests
        public async Task<IActionResult> Index()
        {
              return View(await _context.RoleRequests.Include(r => r.Requester).Include(r => r.Role).ToListAsync());
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null || _context.RoleRequests == null)
            {
                return NotFound();
            }

            var roleRequest = await _context.RoleRequests.Include(rr => rr.Requester).FirstOrDefaultAsync(r => r.Id == id);

            if (roleRequest == null)
            {
                return NotFound();
            }
            try
            {
                roleRequest.IsApproved = true;
                _context.Update(roleRequest);
                await _context.SaveChangesAsync();
                var user = await _userManager.FindByIdAsync(roleRequest.Requester.Id);
                await _userManager.AddToRoleAsync(user, Role.Doctor);
                await _userManager.RemoveFromRoleAsync(user, Role.Guest);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleRequestExists(roleRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        // GET: RoleRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RoleRequests == null)
            {
                return NotFound();
            }

            var roleRequest = await _context.RoleRequests.Include(rr => rr.Requester).FirstOrDefaultAsync(r => r.Id == id);
            roleRequest.UserId = roleRequest.Requester.Id;

            if (roleRequest == null)
            {
                return NotFound();
            }
            return View(roleRequest);
        }

        // POST: RoleRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,IsApproved,DoctorUID,UserId")] RoleRequest roleRequest)
        {
            if (id != roleRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roleRequest);
                    await _context.SaveChangesAsync();
                    var user = await _userManager.FindByIdAsync(roleRequest.UserId);
                    await _userManager.AddToRoleAsync(user, Role.Doctor);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleRequestExists(roleRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(roleRequest);
        }

        // GET: RoleRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RoleRequests == null)
            {
                return NotFound();
            }

            var roleRequest = await _context.RoleRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roleRequest == null)
            {
                return NotFound();
            }

            return View(roleRequest);
        }

        // POST: RoleRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RoleRequests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RoleRequests'  is null.");
            }
            var roleRequest = await _context.RoleRequests.FindAsync(id);
            if (roleRequest != null)
            {
                _context.RoleRequests.Remove(roleRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleRequestExists(int id)
        {
          return _context.RoleRequests.Any(e => e.Id == id);
        }
    }
}
