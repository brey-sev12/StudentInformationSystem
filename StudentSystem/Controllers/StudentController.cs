using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSystem.Models;
using System.Threading.Tasks;

namespace StudentSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly SISDBContext _context;

        // Constructor: Injects the database context (SISDBContext) into the controller
        public StudentController(SISDBContext context)
        {
            _context = context;
        }

        // GET: Student/Index - Displays a list of all students
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync(); // Fetch all students from DB
            return View(students); // Pass the list to the View
        }

        // GET: Student/AddStudent - Returns the form to add a new student
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View(); // Show empty form
        }

        // POST: Student/AddStudent - Handles the form submission for adding a student
        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            // Validate if fields are empty
            if (string.IsNullOrWhiteSpace(student.FirstName))
                ModelState.AddModelError("FirstName", "First name cannot be empty.");

            if (string.IsNullOrWhiteSpace(student.LastName))
                ModelState.AddModelError("LastName", "Last name cannot be empty.");

            // If form data is valid
            if (ModelState.IsValid)
            {
                _context.Students.Add(student); // Add student to DB
                await _context.SaveChangesAsync(); // Save changes
                return RedirectToAction("Index"); // Go back to list
            }

            return View(student); // Return to form with validation errors
        }

        // GET: Student/EditStudent/5 - Show edit form for a student
        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _context.Students.FindAsync(id); // Find student by ID
            if (student == null)
                return NotFound(); // Return 404 if not found

            return View(student); // Show student details in form
        }

        // POST: Student/EditStudent/5 - Save updated student information
        [HttpPost]
        public async Task<IActionResult> EditStudent(int id, Student student)
        {
            if (id != student.StudentID)
                return NotFound(); // Prevent mismatched update

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student); // Mark the student entity as modified
                    await _context.SaveChangesAsync(); // Commit changes
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If student no longer exists (was deleted in the meantime)
                    if (!StudentExists(id))
                        return NotFound();

                    throw; // Rethrow if it’s a different issue
                }
            }

            return View(student); // Return to form with validation messages
        }

        // GET: Student/DeleteStudent/5 - Show confirmation for deletion
        [HttpGet]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id); // Find student by ID
            if (student == null)
                return NotFound();

            return View(student); // Show confirmation view
        }

        // POST: Student/Delete/5 - Actually delete the student from database
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id); // Find student by ID
            if (student != null)
            {
                _context.Students.Remove(student); // Remove from context
                await _context.SaveChangesAsync(); // Commit changes
            }

            return RedirectToAction("Index"); // Go back to student list
        }

        // Helper method: checks if a student exists in the database
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
