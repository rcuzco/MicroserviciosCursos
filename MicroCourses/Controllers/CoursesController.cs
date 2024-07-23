using MicroCourses.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace MicroCourses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CoursesController : ControllerBase
    {
        private const string CoursesFile = "ModelData/Courses.json";

        private static List<Course> LoadCourses()
        {
            if (!System.IO.File.Exists(CoursesFile))
            {
                return [];
            }
            var jsonData = System.IO.File.ReadAllText(CoursesFile);
            if (string.IsNullOrEmpty(jsonData)) return [];
            var courses = JsonSerializer.Deserialize<List<Course>>(jsonData);
            return courses ?? [];
        }

        private static void SaveCourses(List<Course> courses)
        {
            var jsonData = JsonSerializer.Serialize(courses);
            System.IO.File.WriteAllText(CoursesFile, jsonData);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public ActionResult<List<Course>> GetCourses()
        {
            var courses = LoadCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Course> GetCourse(int id)
        {
            var courses = LoadCourses();
            var course = courses.Find(c => c.Id == id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CreatedAtActionResult), StatusCodes.Status200OK)]
        public ActionResult CreateCourse(Course newCourse)
        {
            var courses = LoadCourses();
            newCourse.Id = courses.Count + 1;
            courses.Add(newCourse);
            SaveCourses(courses);
            return CreatedAtAction(nameof(GetCourses), new { id = newCourse.Id }, newCourse);
        }        
    }
}
