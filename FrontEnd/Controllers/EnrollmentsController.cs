using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApiService _apiService;
        private readonly string? _baseUrl;

        public EnrollmentsController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _baseUrl = configuration["Microservices:EnrollmentsService"];
        }

        public async Task<IActionResult> Index()
        {
            var enrollments = await _apiService.GetAsync<List<Enrollment>>(_baseUrl!);
            return View(enrollments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync(_baseUrl!, enrollment);
                return RedirectToAction(nameof(Index));
            }
            return View(enrollment);
        }
    }
}
