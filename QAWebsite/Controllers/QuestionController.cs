using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAchievementDistributor _achievementDistributor;
        private readonly TagController _tagController;
        private readonly RatingController _ratingController;
        private readonly AnswerController _answerController;
        private readonly CommentController _commentController;

        public QuestionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAchievementDistributor achievementDistributor, DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _context = context;
            _userManager = userManager;
            _achievementDistributor = achievementDistributor;
            _tagController = new TagController(context);
            _ratingController = new RatingController(context, userManager, dbContextOptions);
            _answerController = new AnswerController(context, userManager, achievementDistributor, dbContextOptions);
            _commentController = new CommentController(context, userManager, achievementDistributor, dbContextOptions);
        }

        public IEnumerable<IndexViewModel> GetQuestionList(Func<Question, bool> where = null)
        {
            var questions = _context.Question
                .Include(x => x.Author)
                .Include(x => x.Flags)
                .Include(x => x.QuestionTags)
                .ThenInclude(x => x.Tag)
                .AsEnumerable(); //TODO remove in core 2.1 when IQueryable bug is fixed

            if (where != null)
            {
                questions = questions.Where(where);
            }

            return  questions.Select(q => new IndexViewModel(q,
                _ratingController.GetRating<QuestionRating>(q.Id))).ToList()
                .OrderByDescending(q => q.CreationDate);
        }

        // GET: Question
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(GetQuestionList());
        }

        [AllowAnonymous]
        public IActionResult Search(string search)
        {
            if (search == null)
                return RedirectToAction("Index");

            var split = search.Trim().Normalize().Split(' ');

            var questionAnswers = _context.Answer.Where(a => split.Any(s => a.Content.Normalize().Contains(s))).Select(a => a.QuestionId).Distinct();
            
            var vms = GetQuestionList(x => questionAnswers.Any(q => q == x.Id) ||
                split.Any(s => x.Title.Normalize().Contains(s) || 
                               x.Content.Normalize().Contains(s) || 
                               x.QuestionTags.Any(qt => qt.Tag.Name.Normalize().Contains(s)) || 
                               s == x.Author.UserName.Normalize()));

            return View("Index", vms);
        }

        public async Task<DetailsViewModel> GetDetailsViewModel(string questionId)
        {
            if (questionId == null)
            {
                return null;
            }

            var question = await _context.Question
                .Include(x => x.QuestionTags)
                .ThenInclude(x => x.Tag)
                .SingleOrDefaultAsync(m => m.Id == questionId);
            if (question == null)
            {
                return null;
            }
            var avm = _answerController.GetAnswerList(questionId);
            var cvm = _commentController.GetComments<QuestionComment>(questionId);

            return new DetailsViewModel(question, 
                _context.Users.Where(u => u.Id == question.AuthorId).Select(x => x.UserName).SingleOrDefault(),
                _ratingController.GetRating<QuestionRating>(question.Id),
                _context.Flag.Count(f => f.QuestionId == questionId),
                avm, cvm);
        }

        // GET: Question/Details/5
        [AllowAnonymous]
        [Route("/Question/{id:length(8)}")]
        public async Task<IActionResult> Details(string id)
        {
            var vm = await GetDetailsViewModel(id);

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
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

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var question = new Question
            {
                Id = Guid.NewGuid().ToString().Substring(0, 8),
                Title = vm.Title,
                Content = vm.Content,
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                AuthorId = _userManager.GetUserId(User)
            };

            var edit = new QuestionEdit
            {
                Id = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                EditorId = question.AuthorId,
                NewTitle = question.Title,
                NewContent = question.Content,
                EditDate = question.EditDate
            };
                
            _context.Add(question);
            _context.Add(edit);
            await _context.SaveChangesAsync();

            await _tagController.CreateQuestionTags(question, tagNames);
            _achievementDistributor.check(question.AuthorId, _context, AchievementType.QuestionCreation);
            return RedirectToAction(nameof(Index));
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

            var currentUser = _userManager.GetUserAsync(User).Result;
            if (question == null || question.AuthorId != currentUser.Id && !_userManager.IsInRoleAsync(currentUser, Roles.ADMINISTRATOR.ToString()).Result)
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

                var currentUser = _userManager.GetUserAsync(User).Result;
                if (question == null || question.AuthorId != currentUser.Id && !_userManager.IsInRoleAsync(currentUser, Roles.ADMINISTRATOR.ToString()).Result)
                {
                    return NotFound();
                }


                var initialTitle = question.Title;
                var initialContent = question.Content;

                question.Title = vm.Title;
                question.Content = vm.Content;
                question.EditDate = DateTime.Now;

                QuestionEdit edit = new QuestionEdit
                {
                    Id = Guid.NewGuid().ToString(),
                    QuestionId = question.Id,
                    EditorId = currentUser.Id,
                };

                bool editMade = false;

                if (!initialTitle.Equals(question.Title))
                {
                    edit.NewTitle = question.Title;
                    editMade = true;
                }

                if (!initialContent.Equals(question.Content))
                {
                    edit.NewContent = question.Content;
                    editMade = true;
                }

                try
                {

                    await _tagController.UpdateQuestionTags(question, tagNames);

                    if (editMade)
                    {
                        edit.EditDate = question.EditDate;
                        _context.Add(edit);
                    }

                    _context.Update(question);
                    _achievementDistributor.check(currentUser.Id, _context, AchievementType.QuestionEditing);
                    await _context.SaveChangesAsync();
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

        [AllowAnonymous]
        public async Task<IActionResult> EditHistory(string id)
        {
            List<QuestionEditListItem> editsListings = new List<QuestionEditListItem>();
            await _context.QuestionEdits.Where(edit => edit.QuestionId == id).OrderByDescending(edit=>edit.EditDate).ForEachAsync(
                edit => editsListings.Add(new QuestionEditListItem(edit, _context.Users.FirstOrDefault(user => user.Id == edit.EditorId).UserName)));
            editsListings.RemoveAt(editsListings.Count-1);
            return View(new QuestionEditsListViewModel { QuestionId = id, Edits = editsListings});
        }

        [AllowAnonymous]
        public async Task<IActionResult> EditDetails(string id)
        {
            var edit = await _context.QuestionEdits.Where(questionEdit => questionEdit.Id == id).FirstOrDefaultAsync();
            
            var initialTitle =  (edit.NewTitle!=null)?_context.QuestionEdits.Where(questionEdit => questionEdit.QuestionId == edit.QuestionId && questionEdit.NewTitle != null).OrderBy(questionEdit => questionEdit.EditDate).First().NewTitle:null;
            var initialContent = (edit.NewContent!=null)?_context.QuestionEdits.Where(questionEdit => questionEdit.QuestionId == edit.QuestionId && questionEdit.NewContent != null).OrderBy(questionEdit => questionEdit.EditDate).First().NewContent:null;

            return View(new QuestionEditDetailViewModel(edit, initialTitle, initialContent,_context.Users.FirstOrDefault(user => user.Id == edit.EditorId).UserName));
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (question == null || question.AuthorId != currentUser.Id && !_userManager.IsInRoleAsync(currentUser, Roles.ADMINISTRATOR.ToString()).Result)
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
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (question == null || question.AuthorId != currentUser.Id && !_userManager.IsInRoleAsync(currentUser, Roles.ADMINISTRATOR.ToString()).Result)
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
