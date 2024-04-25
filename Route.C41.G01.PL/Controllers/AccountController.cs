using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels.Account;

namespace Route.C41.G01.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;

		private readonly UserManager<ApplicationUser> _UserManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_UserManager = userManager;
			_signInManager = signInManager;
		}
		#region Sign Up
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _UserManager.FindByNameAsync(model.Username);

				if (user is null)
				{
					var User = new ApplicationUser()
					{
						UserName = model.Username,
						FName = model.FirstName,
						LName = model.LastName,
						Email = model.Email,
						IsAgree = model.IsAgree,

					};

					var result = await _UserManager.CreateAsync(User, model.Password);
					if (result.Succeeded)
						return RedirectToAction(nameof(SignIn));

					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Username is already taken");
				}
			}
			return View(model);
		}
		#endregion

		#region Sign In
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _UserManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					var flag = await _UserManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var ressult = await _signInManager.PasswordSignInAsync(user,model.Password, model.RememberMe,false);
						if (ressult.Succeeded)
							return RedirectToAction(nameof(HomeController.Index), "Home");

						if (ressult.IsLockedOut)
							ModelState.AddModelError(string.Empty, "Your account is locked!");
						
						if(ressult.IsNotAllowed)
							ModelState.AddModelError(string.Empty, "Your email is not confirmed!");

					}
				}
				ModelState.AddModelError(string.Empty, "Invalid Login");
			}
			return View(model);
		}

		public async new Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
		#endregion

	}
}
