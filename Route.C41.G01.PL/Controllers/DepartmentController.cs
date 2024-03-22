using Microsoft.AspNetCore.Mvc;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.PL.Controllers
{
    // Inheritance: is Controller
    // Association: has DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var Count = _departmentRepository.Add(department);
                if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            else
            {
                var Department = _departmentRepository.Get(id.Value);
                if (Department is null)
                    return NotFound();

                return View(Department);

            }
        }

    }
}
