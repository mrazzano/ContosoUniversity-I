using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;
using ContosoUniversity.Core.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<Student> _studentRepository;

        public HomeController(IGenericRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Enrollment()
        {
            var students = _studentRepository.Get();
            var enrollments = students
              .GroupBy(x => x.EnrollmentDate)
              .Select(g => new EnrollmentDateViewModel
              {
                  EnrollmentDate = g.Key, 
                  StudentCount = g.Count()
              }).ToList();

            return View(enrollments);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}