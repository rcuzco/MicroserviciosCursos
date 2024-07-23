using Microsoft.AspNetCore.Mvc;
using MicroStudents.Model;
using System.Text.Json;

namespace MicroStudents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class StudentsController : ControllerBase
    {
        private const string StudentsFile = "ModelData/Students.json";
        private static List<Student> LoadStudents()
        {
            if (!System.IO.File.Exists(StudentsFile))
            {
                return [];
            }
            
            var jsonData = System.IO.File.ReadAllText(StudentsFile);
            if (string.IsNullOrEmpty(jsonData)) return [];

            var students = JsonSerializer.Deserialize<List<Student>>(jsonData);            
            return students ?? [];
        }

        private static void SaveStudents(List<Student> students)
        {
            var jsonData = JsonSerializer.Serialize(students);
            System.IO.File.WriteAllText(StudentsFile, jsonData);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public ActionResult<Student> GetStudent(int id)
        {
            var students = LoadStudents();
            var student = students.Find(s => s.Id == id);
            if (student == null) return NotFound();
            return student;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Student>> GetStudents()
        {
            var students = LoadStudents();            
            return students;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public ActionResult<CreatedAtActionResult> CreateStudent(Student newStudent)
        {
            var students = LoadStudents();
            newStudent.Id = students.Count + 1;
            students.Add(newStudent);
            SaveStudents(students);
            return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, newStudent);
        }
    }
}
