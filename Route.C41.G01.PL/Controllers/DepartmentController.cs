using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels;

namespace Route.C41.G01.PL.Controllers
{
    // Inheritance: is Controller
    // Association: has DepartmentRepository
    public class DepartmentController : Controller
    {
		private readonly IMapper _mapper;
		private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;
        public DepartmentController(IMapper mapper, IDepartmentRepository departmentRepository, IWebHostEnvironment env)
        {
			_mapper = mapper;
			_departmentRepository = departmentRepository;
            _env = env;
        }

        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            var MappedDepartments = _mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            return View(MappedDepartments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVm)
        {
            if (ModelState.IsValid)
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                var Count = _departmentRepository.Add(MappedDepartment);
                if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(departmentVm);
        }

        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
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

				var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(Department);
				return View(ViewName, MappedDepartment);

            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, DepartmentViewModel departmentVm)
        {
            if (id != departmentVm.Id)
                return BadRequest();
            

            if (!ModelState.IsValid)
            {
                return View(departmentVm);
            }
            else
            {
                try
                {
					var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
					_departmentRepository.Update(MappedDepartment);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log Exeption
                    // 2. Friendly Message

                    if (_env.IsDevelopment())
                        ModelState.AddModelError(String.Empty, ex.Message);
                    else
                        ModelState.AddModelError(String.Empty, "An Error Has Occured During updating the department");

                    return View(departmentVm);
                }
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Department department = _departmentRepository.Get(id);
            
            if (department is null) return NotFound();

            try
            {
                _departmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exeption
                // 2. Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(String.Empty, ex.Message);
                else
                    ModelState.AddModelError(String.Empty, "An Error Has Occured During Deleting the department");

                var departmentVm = _mapper.Map<Department,DepartmentViewModel>(department);
                return View(departmentVm);
            }
        }
    }
}
