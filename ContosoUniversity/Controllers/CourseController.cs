using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Department> _departmentRepository;

        public CourseController(IGenericRepository<Course> courseRepository, IGenericRepository<Department> departmentRepository)
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
        }

        // GET: Course
        public ActionResult Index(int? departmentId)
        {
            PopulateDepartmentDropDown(departmentId);

            var courses = departmentId.HasValue
                ? _courseRepository.GetBySearch(x => x.DepartmentId == departmentId)
                : _courseRepository.Get();

            return View(courses);
        }

        // GET: Course/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseRepository.GetByIdAsync(id.Value);
            if (course == null)
                return HttpNotFound();

            return View(course);
        }

        public ActionResult Create()
        {
            PopulateDepartmentDropDown(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseId,Title,Credits,DepartmentId")]Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseRepository.AddAsync(course);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateDepartmentDropDown(course.DepartmentId);
            return View(course);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseRepository.GetByIdAsync(id.Value);
            if (course == null)
                return HttpNotFound();

            PopulateDepartmentDropDown(course.DepartmentId);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseRepository.GetByIdAsync(id.Value);
            var fieldsToBind = new[] { "Title", "Credits", "DepartmentId" };
            if (TryUpdateModel(course, fieldsToBind))
            {
                try
                {
                    await _courseRepository.UpdateAsync(course);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateDepartmentDropDown(course.DepartmentId);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseRepository.GetByIdAsync(id.Value);
            if (course == null)
                return HttpNotFound();

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            try
            {
                await _courseRepository.DeleteAsync(course);
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(course);
        }

        private void PopulateDepartmentDropDown(int? departmentId)
        {
            ViewBag.DepartmentId = new SelectList(_departmentRepository.Get(), "DepartmentId", "Name", departmentId);
        }
    }
}
