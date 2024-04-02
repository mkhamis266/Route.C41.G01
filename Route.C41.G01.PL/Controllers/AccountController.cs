using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels.User;

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

		public IActionResult SignUp(SignUpViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = new ApplicationUser()
				{
					UserName = model.Username,
					FName = model.FirstName,
					LName = model.LastName,
					Email = model.Email,
					IsAgree = model.IsAgree,

				};
			}
			return View(model);
		}
		#endregion

		#region Sign In
		public IActionResult SignIn()
		{
			return View();
		}
		#endregion

	}
}
