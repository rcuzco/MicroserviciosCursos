using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public sealed class CoursesController : Controller
    {
        private readonly ApiService _apiService;
        private readonly string _baseUrl;

        public CoursesController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _baseUrl = configuration["Microservices:CoursesService"];
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _apiService.GetAsync<List<Course>>(_baseUrl);
            return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync(_baseUrl, course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
    }
}
