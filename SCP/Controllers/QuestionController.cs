using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCP.Models;

namespace SCP.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository context)
        {
            _questionRepository = context;
        }
        public IActionResult Index()
        {
            List<Question> objQuestion= _questionRepository.GetAll().ToList();
            return View(objQuestion);
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
