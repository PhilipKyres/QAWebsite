﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAchievementDistributor _achievementDistributor;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly RatingController _ratingController;
        private readonly CommentController _commentController;

        public AnswerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAchievementDistributor achievementDistributor, DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _context = context;
            _userManager = userManager;
            _achievementDistributor = achievementDistributor;
            _dbContextOptions = dbContextOptions;
            _ratingController = new RatingController(context, userManager, dbContextOptions);
            _commentController = new CommentController(_context, userManager, achievementDistributor, dbContextOptions);
        }

        // GET: Answer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Answer.ToListAsync());  
        }

        // GET: Answer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsViewModel dvm)
        {
            if (!ModelState.IsValid)
            {
                var newDvm = await new QuestionController(_context, _userManager, _achievementDistributor, _dbContextOptions).GetDetailsViewModel(dvm.Id);
                newDvm.AnswerContent = dvm.AnswerContent;
                return View("~/Views/Question/Details.cshtml", newDvm);
            }

            var answer = new Answer
            {
                Id = Guid.NewGuid().ToString(),
                Content = dvm.AnswerContent,
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                QuestionId = dvm.Id,
                AuthorId = _userManager.GetUserId(User)
            };

            _context.Add(answer);
            await _context.SaveChangesAsync();
            _achievementDistributor.check(answer.AuthorId, _context, AchievementType.AnswerCreation);
            return RedirectToAction("Details", "Question", new { dvm.Id });
        }

        // GET: Answer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Answer am)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid && ModelState.ErrorCount > 1)
            {
                return View(am);
            }

            var answer = await _context.Answer.SingleOrDefaultAsync(a => a.Id == id);
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (answer == null || answer.AuthorId != currentUser.Id && !_userManager.IsInRoleAsync(currentUser, Roles.ADMINISTRATOR.ToString()).Result)
            {
                return NotFound();
            }

            answer.Content = am.Content;
            answer.EditDate = DateTime.Now;

            try
            {
                _context.Update(answer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(am.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "Question", new { Id = answer.QuestionId });
        }

        // GET: Answer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.SingleOrDefaultAsync(m => m.Id == id);

            var currentUser = _userManager.GetUserAsync(User).Result;
            if (answer == null || answer.AuthorId != currentUser.Id && !_userManager.IsInRoleAsync(currentUser, Roles.ADMINISTRATOR.ToString()).Result)
            {
                return NotFound();
            }

            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("details", "Question", new { id = answer.QuestionId });
        }

        private bool AnswerExists(string id)
        {
            return _context.Answer.Any(e => e.Id == id);
        }

        public List<AnswerViewModel> GetAnswerList(string id)
        {
            var answers = _context.Answer.Where(a => a.QuestionId == id).ToList();
			return answers.Select(a => new AnswerViewModel(a, 
			    _context.Users.Where(u => u.Id == a.AuthorId).Select(x => x.UserName).SingleOrDefault(), 
			    _ratingController.GetRating<AnswerRating>(a.Id),
			    _commentController.GetComments<AnswerComment>(a.Id))).ToList();
        }
    }
}
