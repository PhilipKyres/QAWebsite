using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Data;
using QAWebsite.Models;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.QuestionViewModels;

namespace QAWebsite.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TagController _tagController;
        private readonly RatingController _ratingController;

        public QuestionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _tagController = new TagController(context);
            _ratingController = new RatingController(context, userManager);
        }

        // GET: Question
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var questions = await _context.Question
                .Include(x => x.QuestionTags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();

            IEnumerable<QuestionViewModel> vms = questions.Select(q => new QuestionViewModel(q,
                _context.Users.Where(u => u.Id == q.AuthorId).Select(x => x.UserName).SingleOrDefault(),
                _ratingController.GetRating(q.Id)));

            return View(vms);
        }

        // GET: Question/Details/5
        [AllowAnonymous]
        [Route("/Question/{id:length(8)}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(x => x.QuestionTags)
                .ThenInclude(x => x.Tag)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            AnswerController answerController = new AnswerController(_context, _userManager);
            var avm = answerController.GetAnswerList(id);

            var questionViewModel = new DetailsViewModel(question, _context.Users.Where(u => u.Id == question.AuthorId).Select(x => x.UserName).SingleOrDefault(), _ratingController.GetRating(question.Id), avm);

            return View(questionViewModel);
        }

        // GET: Question/Create
        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel vm)
        {
            //Validate here
            var tagNames = _tagController.ValidateParseTags(vm.Tags, ModelState);

            if (ModelState.IsValid)
            {
                var question = new Question
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8),
                    Title = vm.Title,
                    Content = vm.Content,
                    CreationDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    AuthorId = _userManager.GetUserId(User)
                };
                
                _context.Add(question);
                await _context.SaveChangesAsync();

                await _tagController.CreateQuestionTags(question, tagNames);

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(x => x.QuestionTags)
                .ThenInclude(x => x.Tag)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null || question.AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(new EditViewModel(question));
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            var tagNames = _tagController.ValidateParseTags(vm.Tags, ModelState);

            if (ModelState.IsValid)
            {
                var question = await _context.Question
                    .Include(x => x.QuestionTags)
                    .ThenInclude(x => x.Tag)
                    .SingleOrDefaultAsync(m => m.Id == id);
                if (question == null || question.AuthorId != _userManager.GetUserId(User))
                {
                    return NotFound();
                }

                question.Title = vm.Title;
                question.Content = vm.Content;
                question.EditDate = DateTime.Now;

                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();

                    await _tagController.UpdateQuestionTags(question, tagNames);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(vm.Id))
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
            return View(vm);
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null || question.AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null || question.AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SetBestAnswer(string id, string answerId, string best)
        {
            if (answerId == null || id == null || answerId != best && best != null)
            {
                return NotFound();
            }

            var question = await _context.Question.SingleOrDefaultAsync(q => q.Id == id);
            if (question == null || question.AuthorId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            var answer = await _context.Answer.SingleOrDefaultAsync(a => a.QuestionId == id && a.Id == answerId);
            if (answer == null)
            {
                return NotFound();
            }

            question.BestAnswerId = best;

            _context.Update(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }


        private bool QuestionExists(string id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
