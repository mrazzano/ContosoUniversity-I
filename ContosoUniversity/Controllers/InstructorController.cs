using System;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;
using ContosoUniversity.Core.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;
        private readonly IGenericRepository<Department> _departmentRepository;

        public InstructorController(
            IGenericRepository<Course> courseRepository,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Instructor> instructorrRepository)
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _instructorRepository = instructorrRepository;

        }

        // GET: Instructor
        public ActionResult Index(int? id)
        {
            var viewModel = new InstructorDetailViewModel
            {
                Instructors = _instructorRepository.Get()
            };

            if (!id.HasValue)
                return View(viewModel);

            ViewBag.InstructorId = id.Value;
            viewModel.Courses = viewModel.Instructors.Single(i => i.Id == id.Value).Courses;

            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorRepository.GetByIdAsync(id.Value);
            if (instructor == null)
                return HttpNotFound();

            return View(instructor);
        }

        public ActionResult Create()
        {
            var instructor = new Instructor();
            PopulateAssignedCourseData(instructor);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LastName,FirstMidName,HireDate,OfficeAssignment")]Instructor instructor, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                if (selectedCourses != null)
                {

                    foreach (var courseId in selectedCourses.Select(int.Parse))
                    {
                        instructor.Courses.Add(await _courseRepository.GetByIdAsync(courseId));
                    }
                }
                try
                {

                    await _instructorRepository.AddAsync(instructor);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorRepository.GetByIdAsync(id.Value);
            if (instructor == null)
                return HttpNotFound();

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id, string[] selectedCourses)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorRepository.GetByIdAsync(id.Value);
            var fieldsToBind = new[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" };
            if (TryUpdateModel(instructor, fieldsToBind))
            {
                try
                {
                    UpdateInstructorCourses(instructor, selectedCourses);

                    await _instructorRepository.UpdateAsync(instructor);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorRepository.GetByIdAsync(id.Value);
            if (instructor == null)
                return HttpNotFound();

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            instructor.OfficeAssignment = null;
            try
            {
                var department = await _departmentRepository.Get().SingleOrDefaultAsync(d => d.InstructorId == id);
                if (department != null)
                {
                    department.InstructorId = null;
                    await _departmentRepository.UpdateAsync(department);
                }
                await _instructorRepository.DeleteAsync(instructor);
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _courseRepository.Get();
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseId));
            var viewModel = allCourses.Select(course => new AssignedCourseViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.CourseId)
            }).ToList();

            ViewBag.Courses = viewModel;
        }
      
        private void UpdateInstructorCourses(Instructor entity, IReadOnlyCollection<string> courses)
        {
            if (courses == null || courses.Count == 0)
            {
                entity.Courses.Clear();
                return;
            }

            var selectedCourses = new HashSet<int>(courses.Select(c => Convert.ToInt32(c)));
            var instructorCourses = new HashSet<int>(entity.Courses.Select(c => c.CourseId));

            foreach (var course in _courseRepository.Get())
            {
                if (selectedCourses.Contains(course.CourseId))
                {
                    if (!instructorCourses.Contains(course.CourseId))
                    {
                        entity.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseId))
                    {
                        entity.Courses.Remove(course);
                    }
                }
            }
        }
    }
}
