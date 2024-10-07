using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Departments.Include(d => d.Administrator);
            return View(await schoolContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string query = "SELECT * FROM Department WHERE DepartmentID = (0)";
            var department = await _context.Departments
                .FromSqlRaw(query, id)
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Budget, StartDate, RowVersion, InstructorID, DepartmentOwner")] Department Department)
        {

            if (ModelState.IsValid)
            {
                _context.Add(Department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", Department.InstructorID);

            return View(Department);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Department = await _context.Departments.FindAsync(id); //ostsime andmebaasist õpilast id järgi ja paneme ta students nimelisse objekti voi muutujasse
            _context.Departments.Remove(Department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var DepartmentToEdit = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (DepartmentToEdit == null)
            {
                return NotFound();
            }
            return View(DepartmentToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("DepartmentID, Name, Budget, Administrator, StartDate, DepartmentOwner")] Department modifiedDepartment)
        {
            if (ModelState.IsValid)
            {
                if (modifiedDepartment.DepartmentID == null)
                {
                    return BadRequest();
                }
                _context.Departments.Update(modifiedDepartment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(modifiedDepartment);
        }
        [HttpGet]
        public async Task<IActionResult> BaseOn(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments
                .Include(d => d.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);


            if (department == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaseOn(int id, [Bind("DepartmentID, Name, Budget, StartDate, RowVersion, InstructorID, DepartmentOwner")] Department department, string actionType)
        {
            if (ModelState.IsValid)
            {
                var departments = await _context.Departments
                     .FirstOrDefaultAsync(m => m.DepartmentID == id);
                if (actionType == "Make")
                {
                    _context.Add(departments);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else if (actionType == "Make & delete")
                {
                    _context.Departments.Remove(departments);
                    _context.Add(department);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "FullName", department.InstructorID);
            return View(department);
        }

    }
}