using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		private readonly IUnitOfWork _unitOfWork;

		//private readonly IDepartmentRepository _departmentRepository;
		private readonly IWebHostEnvironment _env;
        public DepartmentController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env
            /*IDepartmentRepository departmentRepository*/
            )
        {
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			//_departmentRepository = departmentRepository;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync();
            var MappedDepartments = _mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            return View(MappedDepartments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVm)
        {
            if (ModelState.IsValid)
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                 _unitOfWork.Repository<Department>().Add(MappedDepartment);
                var Count = await _unitOfWork.Complete();

				if (Count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(departmentVm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            else
            {
                var Department = await _unitOfWork.Repository<Department>().GetAsync(id.Value);
                if (Department is null)
                    return NotFound();

				var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(Department);
				return View(ViewName, MappedDepartment);

            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, DepartmentViewModel departmentVm)
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
					_unitOfWork.Repository<Department>().Update(MappedDepartment);
                    await _unitOfWork.Complete();
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
        public async Task<IActionResult> Delete(int id)
        {
            Department department = await _unitOfWork.Repository<Department>().GetAsync(id);
            
            if (department is null) return NotFound();

            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
                await _unitOfWork.Complete();
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
