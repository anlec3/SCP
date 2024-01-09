using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SCP.Models;
using System.Drawing;
using System.Security.Policy;

namespace SCP.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly AppDbContext _appDbContext;

        public AnswerController(IAnswerRepository context, IQuestionRepository questionRepository, AppDbContext appDbContext)
        {
            _answerRepository = context;
            _questionRepository = questionRepository;
            _appDbContext = appDbContext;
        }
        public IActionResult Index(string searchText = "", int page = 1, int size = 6)
        {
            var answer = _appDbContext.Answers.Where(s => s.Text.ToLower().Contains(searchText.ToLower())).AsQueryable();

            int pageskip = (page - 1) * size;
            var Answer = answer.Skip(pageskip).Take(size).Select(x => new Answer()
            {
                Id = x.Id,
                Text = x.Text,
                UserName = x.UserName,

            }).ToList();
            int recordCount = answer.Count();
            int pageCount = (int)Math.Ceiling(Convert.ToDecimal(recordCount / size));


            ViewBag.PageCount = pageCount;
            ViewBag.RecordCount = recordCount;
            ViewBag.Page = page;
            ViewBag.Size = size;
            ViewBag.SearchText = searchText;

            return View(Answer);
        }
        
        [Authorize]
        public IActionResult Add()
        {
            IEnumerable<SelectListItem> QuestionList = _questionRepository.GetAll()
                .Select(q => new SelectListItem
                {
                    Text = q.Text,
                    Value = q.Id.ToString(),
                });

            ViewBag.QuestionList = QuestionList;
            return View();
        }

        [HttpPost]
        public IActionResult Add(Answer answer)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);

            if (ModelState.IsValid)
            {
                _answerRepository.Add(answer);
                _answerRepository.Save();
                TempData["success"] = "Cevabınız Eklendi!";
                return RedirectToAction("Index", "Question");
            }
            ViewBag.QuestionId = answer.QuestionId;
            return View();

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Answer? answerdb = _answerRepository.Get(u => u.Id == id);
            if (answerdb == null)
            {
                return NotFound();
            }
            return View(answerdb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult PostDelete(int? id)
        {
            Answer? answer = _answerRepository.Get(u => u.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            _answerRepository.Delete(answer);
            _answerRepository.Save();
            return RedirectToAction("Index", "Answer");
        }

        public IActionResult GetQuestionAndAnswers(int questionId, string searchText = "", int page = 1, int size = 6)
        {
            List<Answer> objAnswer = _appDbContext.Answers
                                .Where(a => a.QuestionId == questionId)
                                .ToList();

            var answer = _appDbContext.Answers.Where(s => s.Text.ToLower().Contains(searchText.ToLower())).AsQueryable();

            int pageskip = (page - 1) * size;
            var Answer = answer.Skip(pageskip).Take(size).Select(x => new Answer()
            {
                Id = x.Id,
                Text = x.Text,
                UserName = x.UserName,

            }).ToList();
            int recordCount = answer.Count();
            int pageCount = (int)Math.Ceiling(Convert.ToDecimal(recordCount / size));


            ViewBag.PageCount = pageCount;
            ViewBag.RecordCount = recordCount;
            ViewBag.Page = page;
            ViewBag.Size = size;
            ViewBag.SearchText = searchText;

            return View(objAnswer);
        }
    }
}
