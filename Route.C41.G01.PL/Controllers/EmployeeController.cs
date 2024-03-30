using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels;

namespace Route.C41.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
		private readonly IMapper _mapper;

		//private readonly IDepartmentRepository _departmentRepository;
		//private readonly IEmployeeRepository _employeeRepository;

        private readonly IHostEnvironment _env;
		private readonly IUnitOfWork _unitOfWork;

		public EmployeeController(
            IMapper mapper, 
            IHostEnvironment env,
            IUnitOfWork unitOfWork
            /*IEmployeeRepository employeeRepository*/
            /*IDepartmentRepository departmentRepository*/
            )
        {
			_mapper = mapper;
			//_departmentRepository = departmentRepository;
			//_employeeRepository = employeeRepository;
            _env = env;
			_unitOfWork = unitOfWork;
		}

        public IActionResult Index(string searchInp)
        {
            // if you want to keep the temp data in another redirection 
            TempData.Keep();
            // Bind Through View's Dictionary : Transfer Data from Action to View [one way]
            // 1. viewData: is a Dictionary Type Property (introduced in ASP.Net Framwork 3.5)
            //   => it help us to transfer data from Controller[Action] to view
            ViewData["Message"] = "Hello From View Data";

            // 2. viewBag: Dynamic type property (introduced in ASP.Net Framwork 4.0 based on dynamic type)
            //   => it help us to transfer data from Controller[Action] to view

            ViewBag.Message = "Hello From View Bag";
            var employees = Enumerable.Empty<Employee>();

            if (String.IsNullOrEmpty(searchInp))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(searchInp);
            }

            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
           return View(MappedEmployees);
        }

        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            else
            {
                var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
                if (employee is null)
                    return NotFound();

                var MappedEmp = _mapper.Map<Employee,EmployeeViewModel>(employee);
                return View(ViewName, MappedEmp);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            //ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {


                // TempData: is Dictionary Type Property (introduced in ASP.net Framwork 3.5)
                //         => used  to pass data between two consecutive requests.

                //Manual Mapping
                ///var mappedEmployee = new Employee()
                ///{
                ///    Id = employeeVM.Id,
                ///    Name = employeeVM.Name,
                ///    Age = employeeVM.Age,
                ///    Address = employeeVM.Address,
                ///    Salary = employeeVM.Salary,
                ///    Email = employeeVM.Email,
                ///    PhoneNumber = employeeVM.PhoneNumber,
                ///    IsActive = employeeVM.IsActive,
                ///    HiringDate = employeeVM.HiringDate,
                ///};

                ///var mappedEmployee = (Employee)employeeVM;


                var MappedEmp = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
				 _unitOfWork.EmployeeRepository.Add(MappedEmp);
                var count = _unitOfWork.Complete();

                if (count > 0)
                    TempData["Message"] = "Department Created Succeefuly";

                else
                    TempData["Message"] = "An Error Occured";

                return RedirectToAction("Index");
            }
            return View(employeeVM);
        }

        public IActionResult Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepository.GetAll();
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVm)
        {
            if (id != employeeVm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employeeVm);
            else
            {
                try
                {
                    var MappedEmp = _mapper.Map<EmployeeViewModel,Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Update(MappedEmp);
                    _unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // 1. Log Exeption
                    // 2. Friendly Message

                    if (_env.IsDevelopment())
                        ModelState.AddModelError(String.Empty, ex.Message);
                    else
                        ModelState.AddModelError(String.Empty, "An Error Has Occured During updating the Employee");

                    return View(employeeVm);
                }
            }

        }


        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVm)
        {

            try
            {
				var MappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
				_unitOfWork.EmployeeRepository.Delete(MappedEmp);
                _unitOfWork.Complete();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                // 1. Log Exeption
                // 2. Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(String.Empty, ex.Message);
                else
                    ModelState.AddModelError(String.Empty, "An Error Has Occured During deleting the Employee");

                return View(employeeVm);
            }
        }
    }
}
