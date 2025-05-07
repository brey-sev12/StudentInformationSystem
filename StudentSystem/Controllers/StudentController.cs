using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using StudentSystem.Models;

namespace StudentSystem.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> students = new List<Student>();

        [HttpGet]
        public IActionResult Index()
        {
            return View(students);
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {

            if (student.FullName == "")
            {
                ModelState.AddModelError("Name", "Name cannot be empty.");
            }

            if (ModelState.IsValid)
            {
                student.StudentID = students.Count + 1;
                students.Add(student);
                return RedirectToAction("Index");

            }
            return View(student);
        }

        
        public IActionResult EditStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.StudentID ==id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        //POST: /Students/Edit/id
        [HttpPost]
        public IActionResult EditStudent(int id, Student student) 
        { 
            if (id != student.StudentID)
                return NotFound();

            if(ModelState.IsValid)
            {
                var existingStudent = students.FirstOrDefault(s => s.StudentID == id);
                if(existingStudent == null)
                    return NotFound();

                //Update student details
                existingStudent.StudentID = student.StudentID;
                existingStudent.FirstName = student.FirstName;
                existingStudent.MiddleName = student.MiddleName;
                existingStudent.LastName = student.LastName;
                existingStudent.Sex = student.Sex;
                existingStudent.CivilStatus = student.CivilStatus;
                existingStudent.Contact = student.Contact;
                existingStudent.Address = student.Address;

                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: /Students/Delete/id
        public IActionResult DeleteStudent(int id) 
        { 
            var student = students.FirstOrDefault(s => s.StudentID == id);
            if (student == null) 
                return NotFound();
            return View(student);
        }

        //  POST: /Students/Delete/id
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = students.FirstOrDefault(s => s.StudentID == id);
            if (student != null)
            {
                students.Remove(student);
            }
            return RedirectToAction("Index");
        }
    }
}
