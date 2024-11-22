
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var vm = new InstructorIndexData();
            vm.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Enrollments)
                .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = vm.Instructors
                    .Where(i => i.ID == id.Value).Single();
                vm.Courses = instructor.CourseAssignments
                    .Select(i => i.Course);
            }
            if (courseId != null)
            {
                ViewData["CourseID"] = courseId.Value;
                vm.Enrollments = vm.Courses
                    .Where(x => x.CourseID == courseId)
                    .Single()
                    .Enrollments;
            }

            return View(vm);

        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var InstructorToEdit = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (InstructorToEdit == null)
            {
                return NotFound();
            }
            return View(InstructorToEdit);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID,LastName,FirstMidName,EnrollmentDate")] Instructor modifiedInstructor)
        {
            if (ModelState.IsValid)
            {
                if (modifiedInstructor.ID == null)
                {
                    return BadRequest();
                }
                _context.Instructors.Update(modifiedInstructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(modifiedInstructor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor/*, string? selectedCourses*/)
        {
            //if (selectedCourses == null)
            //{
            //    instructor.CourseAssignments = new List<CourseAssignment>();
            //    foreach (var course in selectedCourses)
            //    {
            //        var courseToAdd = new CourseAssignment
            //        {
            //            InstructorID = instructor.ID,
            //            CourseID = course
            //        };
            //        instructor.CourseAssignments.Add(courseToAdd);
            //    }
            //}
            //ModelState.Remove();
            //ModelState.Remove(selectedCourses);
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //PopulateAsjah kannatab
            //signedCourseData(instructor); //uuendab instructori juures olevaid kursuseid
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses; //leiame kõik kursused
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            //valime kursused kus courseid on õpetajal olemas
            var vm = new List<AssignedCourseData>(); //teeme viewmodeli jaoks uue nimekirja
            foreach (var course in allCourses)
            {
                vm.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewData["Courses"] = vm;
        }

        

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}