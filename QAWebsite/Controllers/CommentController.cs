using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.QuestionViewModels;

namespace QAWebsite.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsViewModel dvm, string parentId, CommentTypes type)
        {
            if (dvm.Comment == null || dvm.Comment.Trim().Length == 0 || parentId == null || type != CommentTypes.Question && type != CommentTypes.Answer)
            {
                var newDvm = await new QuestionController(_context, _userManager).GetDetailsViewModel(dvm.Id);
                newDvm.AnswerContent = dvm.AnswerContent;
                return View("~/Views/Question/Details.cshtml", newDvm);
            }

            Comment comment;

            if (type == CommentTypes.Question)
            {
                comment = new QuestionComment
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = dvm.Comment,
                    CreationDate = DateTime.Now,
                    QuestionId = parentId,
                    AuthorId = _userManager.GetUserId(User)
                };
            }

            else
            {
                comment = new AnswerComment
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = dvm.Comment,
                    CreationDate = DateTime.Now,
                    Answerid = parentId,
                    AuthorId = _userManager.GetUserId(User)
                };
            }

            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Question", new { dvm.Id });
        }

        public List<CommentViewModel> GetQuestionCommentsList(string id)
        {
            var comments = _context.QuestionComment.Where(q => q.QuestionId == id).OrderBy(o => o.CreationDate).ToList();

            return BuildCommentList(comments, id, CommentTypes.Question);
        }

        public List<CommentViewModel> GetAnmswerCommentList(string id)
        {
            var comments = _context.AnswerComment.Where(a => a.Answerid == id).OrderBy(o => o.CreationDate).ToList();

            return BuildCommentList(comments, id, CommentTypes.Answer);
        }

        private List<CommentViewModel> BuildCommentList(IEnumerable<Comment> comments, string id, CommentTypes type)
        {
            List<CommentViewModel> cvm = new List<CommentViewModel>();

            foreach (Comment c in comments)
            {
                string name = _context.Users.Where(u => u.Id == c.AuthorId).Select(x => x.UserName).SingleOrDefault();
                cvm.Add(new CommentViewModel(c, name, id, type));
            }

            return cvm;
        }



        /*
        // GET: Comment
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comment.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,CreationDate,AuthorId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Content,CreationDate,AuthorId")] Comment comment)
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
            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.Id == id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(string id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }*/
    }
}
