using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;

namespace ContosoUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;

        public DepartmentController(IGenericRepository<Department> departmentRepository, IGenericRepository<Instructor> instructorRepository)
        {
            _departmentRepository = departmentRepository;
            _instructorRepository = instructorRepository;
        }

        // GET: Department
        public ActionResult Index()
        {
            var departments = _departmentRepository.Get();
            return View(departments);
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentRepository.GetByIdAsync(id.Value);
            if (department == null)
                return HttpNotFound();

            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            PopulateInstructorDropdown(null);
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentId,Name,Budget,StartDate,InstructorId")] Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentRepository.AddAsync(department);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateInstructorDropdown(department.InstructorId);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentRepository.GetByIdAsync(id.Value);
            if (department == null)
                return HttpNotFound();

            PopulateInstructorDropdown(department.InstructorId);
            return View(department);
        }

        // POST: Department/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentRepository.GetByIdAsync(id.Value);
            string[] fieldsToBind = { "Name", "Budget", "StartDate", "InstructorId", };
            if (TryUpdateModel(department, fieldsToBind))
            {
                try
                {
                    await _departmentRepository.UpdateAsync(department);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateInstructorDropdown(department.InstructorId);
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentRepository.GetByIdAsync(id.Value);
            if (department == null)
                return HttpNotFound();

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            try
            {
                await _departmentRepository.DeleteAsync(department);
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(department);
        }

        private void PopulateInstructorDropdown(int? instructorId)
        {
            ViewBag.InstructorId = new SelectList(_instructorRepository.Get(), "Id", "FullName", instructorId);
        }
    }
}
