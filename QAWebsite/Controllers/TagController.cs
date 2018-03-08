using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Extensions;
using QAWebsite.Models.QuestionViewModels;

namespace QAWebsite.Controllers
{
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tag
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tag.Include(x => x.QuestionTags)
                .Select(t => new TagViewModel(t, t.QuestionTags.Count))
                .ToListAsync());
        }

        public IEnumerable<string> ValidateParseTags(string commaDelimitedTags, ModelStateDictionary modelState)
        {
            if (string.IsNullOrWhiteSpace(commaDelimitedTags))
            {
                return Enumerable.Empty<string>();
            }

            var tags = commaDelimitedTags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(s => s != string.Empty).Distinct();

            // Validate tags? If bad, invalidate the model state
            string tag = tags.FirstOrDefault(x => x.Length > 35);
            if (tag != null)
            {
                modelState.AddModelError("Tags", $"The tag \n'{tag}'\nis too long; the maximum length is 35 characters.");
            }

            return tags;
        }

        public async Task CreateQuestionTags(Question question, IEnumerable<string> tagNames)
        {
            foreach (var name in tagNames)
            {
                Tag tag = new Tag { Id = Guid.NewGuid().ToString(), Name = name };
                tag = _context.Tag.AddIfNotExists(tag, t => t.Name == name);
                _context.QuestionTag.Add(new QuestionTag { Question = question, Tag = tag });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionTags(Question existingQuestion, IEnumerable<string> newTagNames)
        {
            var names = new HashSet<string>(newTagNames);
            var toRemove = existingQuestion.QuestionTags.Where(x => !names.Contains(x.Tag.Name));
            _context.QuestionTag.RemoveRange(toRemove);

            await CreateQuestionTags(existingQuestion, names.Except(existingQuestion.QuestionTags.Select(x => x.Tag.Name)));
        }

        // GET: Tag/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("SoLost");
            }

            var tag = await _context.Tag
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return View("SoLost");
            }

            return View(tag);
        }

        // POST: Tag/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tag = await _context.Tag.SingleOrDefaultAsync(m => m.Id == id);
            _context.Tag.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(string id)
        {
            return _context.Tag.Any(e => e.Id == id);
        }
    }
}
