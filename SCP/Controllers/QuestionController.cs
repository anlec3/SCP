using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCP.Models;

namespace SCP.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly AppDbContext _appDbContext;

        public QuestionController(IQuestionRepository context, AppDbContext appDbContext)
        {
            _questionRepository = context;
            _appDbContext = appDbContext;
        }
        public IActionResult Index(string searchText = "", int page = 1, int size = 6)
        {
            var question = _appDbContext.Questions.Where(s => s.Text.ToLower().Contains(searchText.ToLower())).AsQueryable();

            int pageskip = (page - 1) * size;
            var Question = question.Skip(pageskip).Take(size).Select(x => new Question()
            {
                Id = x.Id,
                Text = x.Text,
                UserName = x.UserName,
               
            }).ToList();
            int recordCount = question.Count();
            int pageCount = (int)Math.Ceiling(Convert.ToDecimal(recordCount / size));


            ViewBag.PageCount = pageCount;
            ViewBag.RecordCount = recordCount;
            ViewBag.Page = page;
            ViewBag.Size = size;
            ViewBag.SearchText = searchText;

            return View(Question);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Add(Question question)
        {
            if (ModelState.IsValid)
            {
                _questionRepository.Add(question);
                _questionRepository.Save();
                TempData["success"] = "Yeni Soru Eklendi!";
                return RedirectToAction("Index", "Question");
            }
            return View();
           
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {  
                return NotFound(); 
            }
            Question? questiondb = _questionRepository.Get(u=>u.Id==id);
            if (questiondb == null) 
            {
                return NotFound();
            }
            return View(questiondb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult PostDelete(int? id)
        {
            Question? question = _questionRepository.Get(u=>u.Id==id);
            if (question == null)
            {
                return NotFound();
            }
            _questionRepository.Delete(question);
            _questionRepository.Save();
            TempData["success"] = "Soru Silindi!";
            return RedirectToAction("Index", "Question");
        }
        
    }
}
