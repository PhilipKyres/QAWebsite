﻿using System;
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

        public QuestionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _tagController = new TagController(context);
        }

        // GET: Question
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Question
                .Include(x => x.QuestionTags)
                .ThenInclude(x => x.Tag)
                .ToListAsync());
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

            return View(question);
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

        private bool QuestionExists(string id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}