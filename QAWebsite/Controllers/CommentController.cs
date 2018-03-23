using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.QuestionViewModels;
using QAWebsite.Models.UserModels;
using QAWebsite.Services;

namespace QAWebsite.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAchievementDistributor _achievementDistributor;

        public CommentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAchievementDistributor achievementDistributor)
        {
            _context = context;
            _userManager = userManager;
            _achievementDistributor = achievementDistributor;
        }

        public List<CommentViewModel> GetComments<T>(DbSet<T> dbSet, string id) where T : Comment
        {
            var comments = dbSet.Where(c => c.FkId == id).OrderBy(o => o.CreationDate).ToList();

            List<CommentViewModel> cvm = new List<CommentViewModel>();

            foreach (T c in comments)
            {
                string name = _context.Users.Where(u => u.Id == c.AuthorId).Select(x => x.UserName).SingleOrDefault();
                cvm.Add(new CommentViewModel(c, name));
            }

            return cvm;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsViewModel dvm, string parentId, CommentTypes type)
        {
            if (dvm.Comment == null || dvm.Comment.Trim().Length == 0 || parentId == null || type != CommentTypes.Question && type != CommentTypes.Answer)
            {
                var newDvm = await new QuestionController(_context, _userManager, _achievementDistributor).GetDetailsViewModel(dvm.Id);
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
                    FkId = parentId,
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
                    FkId = parentId,
                    AuthorId = _userManager.GetUserId(User)
                };
            }

            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Question", new { dvm.Id });
        }

        public async Task<IActionResult> Delete(string id, string questionId, CommentTypes type)
        {
            if (id == null)
                return NotFound();

            Comment comment;

            if (type == CommentTypes.Question)
                comment = await _context.QuestionComment.SingleOrDefaultAsync(c => c.Id == id);

            else
                comment = await _context.AnswerComment.SingleOrDefaultAsync(c => c.Id == id);             

            if (comment == null)
                return NotFound();

            string name = _context.Users.Where(u => u.Id == comment.AuthorId).Select(x => x.UserName).SingleOrDefault();
            CommentViewModel cvm = new CommentViewModel(comment, name)
            {
                Type = type,
                ParentId = questionId               
            };
            return View(cvm);
        }


        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string parentId, CommentTypes type)
        {

            if (id == null || parentId == null)
                return NotFound();

            if (type == CommentTypes.Question)
            {
               var comment = await _context.QuestionComment.SingleOrDefaultAsync(c => c.Id == id);
                _context.QuestionComment.Remove(comment);
            }

            else
            {
                var comment = await _context.AnswerComment.SingleOrDefaultAsync(c => c.Id == id);
                _context.AnswerComment.Remove(comment);
            }
                    
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Question", new { id = parentId });
        }     
    }
}
