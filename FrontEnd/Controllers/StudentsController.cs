using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public sealed class StudentsController : Controller
    {
        private readonly ApiService _apiService;
        private readonly string? _baseUrl;

        public StudentsController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _baseUrl = configuration["Microservices:StudentsService"];
        }

        public async Task<IActionResult> Index()
        {
            var students = await _apiService.GetAsync<List<Student>>(_baseUrl!);
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync(_baseUrl!, student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
    }
}
