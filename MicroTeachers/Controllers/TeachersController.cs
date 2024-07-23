using Microsoft.AspNetCore.Mvc;
using MicroTeachers.Model;
using System.Text.Json;

namespace MicroTeachers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TeachersController : ControllerBase
    {
        private const string TeachersFile = "ModelData/Teachers.json";

        private static List<Teacher> LoadTeachers()
        {
            if (!System.IO.File.Exists(TeachersFile))
            {
                return [];
            }
            var jsonData = System.IO.File.ReadAllText(TeachersFile);
            if (string.IsNullOrEmpty(jsonData)) return [];

            var teachers = JsonSerializer.Deserialize<List<Teacher>>(jsonData);
            return teachers ?? [];
        }

        private static void SaveTeachers(List<Teacher> teachers)
        {
            var jsonData = JsonSerializer.Serialize(teachers);
            System.IO.File.WriteAllText(TeachersFile, jsonData);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public ActionResult<Teacher> GetTeacher(int id)
        {
            var teachers = LoadTeachers();
            var teacher = teachers.Find(t => t.Id == id);
            if (teacher == null) return NotFound();
            return Ok(teacher);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Teacher>> GetTeachers()
        {
            var teachers = LoadTeachers();            
            return Ok(teachers);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public ActionResult<Teacher> CreateTeacher(Teacher newTeacher)
        {
            var teachers = LoadTeachers();
            newTeacher.Id = teachers.Count + 1;
            teachers.Add(newTeacher);
            SaveTeachers(teachers);
            return CreatedAtAction(nameof(GetTeacher), new { id = newTeacher.Id }, newTeacher);
        }
    }
}
