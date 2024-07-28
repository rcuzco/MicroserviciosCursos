using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public sealed class TeachersController : Controller
    {
        private readonly ApiService _apiService;
        private readonly string? _baseUrl;

        public TeachersController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _baseUrl = configuration["Microservices:TeachersService"];
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _apiService.GetAsync<List<Teacher>>(_baseUrl!);
            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync(_baseUrl!, teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }
    }
}
