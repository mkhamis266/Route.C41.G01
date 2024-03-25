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
			// if you want to keep the temp data in another redirection 
			TempData.Keep(); 
			// Bind Through View's Dictionary : Transfer Data from Action to View [one way]
			// 1. viewData: is a Dictionary Type Property (introduced in ASP.Net Framwork 3.5)
			//   => it help us to transfer data from Controller[Action] to view
			ViewData["Message"] = "Hello From View Data";

            // 2. viewBag: Dynamic type property (introduced in ASP.Net Framwork 4.0 based on dynamic type)
            //   => it help us to transfer data from Controller[Action] to view

            ViewBag.Message = "Hello From View Bag";
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
				// TempData: is Dictionary Type Property (introduced in ASP.net Framwork 3.5)
				//         => used  to pass data between two consecutive requests.
                var count = _employeeRepository.Add(employee);
				if (count > 0)
					TempData["Message"] = "Department Created Succeefuly";
				
				else
                    TempData["Message"] = "An Error Occured";
                
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
