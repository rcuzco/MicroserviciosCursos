using MicroEnrollments.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroEnrollments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class EnrollmentsController(HttpClient httpClient) : ControllerBase
    {
        private const string EnrollmentsFile = "ModelData/Enrollments.json";
        private readonly HttpClient _httpClient = httpClient;

        private static List<Enrollment> LoadEnrollments()
        {
            if (!System.IO.File.Exists(EnrollmentsFile))
            {
                return [];
            }
            var jsonData = System.IO.File.ReadAllText(EnrollmentsFile);
            if (string.IsNullOrEmpty(jsonData)) return [];

            var enrollments = JsonSerializer.Deserialize<List<Enrollment>>(jsonData);
            return enrollments ?? [];
        }

        private static void SaveEnrollments(List<Enrollment> enrollments)
        {
            var jsonData = JsonSerializer.Serialize(enrollments);
            System.IO.File.WriteAllText(EnrollmentsFile, jsonData);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Enrollment> GetEnrollment(int id)
        {
            var enrollments = LoadEnrollments();
            var enrollment = enrollments.Find(e => e.Id == id);
            if (enrollment == null) return NotFound();
            return enrollment;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateEnrollment(Enrollment newEnrollment)
        {
            var enrollments = LoadEnrollments();

            // Verify student exists
            var studentResponse = await _httpClient.GetAsync($"http://localhost:6187/api/students/{newEnrollment.StudentId}");
            if (!studentResponse.IsSuccessStatusCode)
            {
                return BadRequest("Invalid Student");
            }

            // Verify course exists
            var courseResponse = await _httpClient.GetAsync($"http://localhost:6156/api/courses/{newEnrollment.CourseId}");
            if (!courseResponse.IsSuccessStatusCode)
            {
                return BadRequest("Invalid Course");
            }

            newEnrollment.Id = enrollments.Count + 1;
            enrollments.Add(newEnrollment);
            SaveEnrollments(enrollments);
            return CreatedAtAction(nameof(GetEnrollment), new { id = newEnrollment.Id }, newEnrollment);
        }        
    }
}
