using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IHostEnvironment _env;

		public EmployeeController(IEmployeeRepository employeeRepository, IHostEnvironment env) 
		{
			_employeeRepository = employeeRepository;
			_env = env;
		}

		public IActionResult Index()
		{
			var employees = _employeeRepository.GetAll();
			return View(employees);
		}

		public IActionResult Details(int? id, string ViewName = "Details")
		{
			if (!id.HasValue)
				return BadRequest();
			else
			{
				var employee = _employeeRepository.Get(id.Value);
				if (employee is null)
					return NotFound();

				return View(ViewName, employee);
			}
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Employee employee) 
		{
            if (ModelState.IsValid)
            {
                var count = _employeeRepository.Add(employee);
				if (count > 0) 
					return RedirectToAction("Index");
            }
			return View(employee);
        }

		public IActionResult Edit(int? id) 
		{
			return Details(id, "Edit");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit([FromRoute]int id,Employee employee)
		{
			if(id != employee.Id)
				return BadRequest();

			if (!ModelState.IsValid)
				return View(employee);
			else
			{
				try
				{
					_employeeRepository.Update(employee);
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

					return View(employee);
				}
			}

		}


		public IActionResult Delete(int? id) 
		{
			return Details(id, "Delete");
		}
		[HttpPost]
		public IActionResult Delete(Employee employee)
		{
			 
			try
			{
				_employeeRepository.Delete(employee);
				return RedirectToAction("index");
			}
			catch(Exception ex)
			{
				// 1. Log Exeption
				// 2. Friendly Message

				if (_env.IsDevelopment())
					ModelState.AddModelError(String.Empty, ex.Message);
				else
					ModelState.AddModelError(String.Empty, "An Error Has Occured During deleting the Employee");

				return View(employee);
			}
		}
	}
}
