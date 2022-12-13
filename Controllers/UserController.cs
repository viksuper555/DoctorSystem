using DoctorSystem.Data;
using DoctorSystem.Models;
using DoctorSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DoctorSystem.Controllers;


public class UserController : Controller
{
    private readonly UserManager<DefaultUser> _userManager;
    private readonly ApplicationDbContext _context;
    private static readonly string DOCTOR_ROLE_NAME = "DOCTOR";

    public UserController(UserManager<DefaultUser> userManager, ApplicationDbContext context) 
    { 
        _userManager = userManager;
        _context = context;
    }
    // GET: UserController
    public async Task<IActionResult> Index()
    {
        var doctors = _context.Users
           .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
           .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
           .Where(u => u.r.NormalizedName == DOCTOR_ROLE_NAME)
           .Select(c => new UserViewModel()
           {
               Id = c.ur.u.Id,
               FirstName = c.ur.u.FirstName,
               LastName = c.ur.u.LastName,
               Email = c.ur.u.Email
           }).ToList().GroupBy(uv => new { uv.Id, uv.FirstName, uv.LastName, uv.Email })
           .Select(r => new UserViewModel()
           {
               Id = r.Key.Id,
               FirstName = r.Key.FirstName,
               LastName = r.Key.LastName,
               Email = r.Key.Email
           }).ToList();


        return View(doctors);
    }

    // GET: UserController/Details/5
    public async Task<ActionResult> Details(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (!await _userManager.IsInRoleAsync(user, DOCTOR_ROLE_NAME))
            return RedirectToAction(nameof(Index));
        return View(new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender
        });
    }

    // GET: UserController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: UserController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: UserController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: UserController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: UserController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: UserController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
