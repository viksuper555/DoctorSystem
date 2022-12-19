using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorSystem.Data;
using DoctorSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DoctorSystem.Data.Migrations;
using Category = DoctorSystem.Models.Category;
using Post = DoctorSystem.Models.Post;
using DoctorSystem.Misc;
using Microsoft.Extensions.Hosting;

namespace DoctorSystem.Controllers
{
    
    //[Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        
        private readonly UserManager<DefaultUser> _userManager;

        private readonly ApplicationDbContext _context;


        public PostsController(ApplicationDbContext context, UserManager<DefaultUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ViewResult Index(string searchString)
        {
            var str = _context.Post
                        .Include(p => p.Comments)
                        .ThenInclude(x => x.Creator)
                        .Include(t => t.Creator)
                        .OrderByDescending(s => s.DateCreated)
                        .Include(v => v.Category).ToList();
            if (User.IsInRole(Role.Doctor))
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    return View(str.Where(s => s.Category.Name == searchString));
                }
                return View(str);
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    return View(str.Where(s => s.Creator.Email == User.Identity.Name)
                        .Where(s => s.Category.Name == searchString));
                }
                return View(str.Where(s => s.Creator.Email == User.Identity.Name));
            }
        }
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.Include(p=>p.Comments).ThenInclude(c=>c.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var res = new PostDetailViewModel()
            {
                Post = post,
                PostId = post.Id,
            };
            return View(res);

        }
        public async Task<IActionResult> CreateComment(PostDetailViewModel res, Comment comment)
        {
            comment.Creator = await _userManager.GetUserAsync(User);
            comment.Post = await _context.Post.FindAsync(res.PostId);
            comment.CreatedAt = DateTime.Now;
            comment.Text = res.Text;

            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Patient")]
        // GET: Posts/Create
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _context.Category.Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            ViewBag.CategoryList = CategoryList;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DateCreated,CategoryId")] Post post)
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", post.CategoryId);
            post.Category = await _context.FindAsync<Category>(post.CategoryId);
            ModelState.Remove("Category");
            post.Creator = await _userManager.GetUserAsync(User);
            post.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewBag.CategoryList = CategoryList;
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DateCreated")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Post == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Post'  is null.");
            }
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditComment(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Description", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, [Bind("Id,PostId,CreatedAt,Text")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["PostId"] = new SelectList(_context.Post, "Id", "Description", comment.PostId);
            return View(comment);
        }

        public async Task<IActionResult> DeleteComment(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCommentConfirmed(int id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return _context.Post.Any(e => e.Id == id);
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}

