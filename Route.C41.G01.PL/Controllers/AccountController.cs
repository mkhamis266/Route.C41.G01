using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Services.EmailSender;
using Route.C41.G01.PL.ViewModels.Account;

namespace Route.C41.G01.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configs;
		private readonly UserManager<ApplicationUser> _UserManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IEmailSender emailSender,IConfiguration configs )
		{
			_UserManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_configs = configs;
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
        public IActionResult ForgetPassword()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _UserManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var resetPasswordToken = await _UserManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new {email = user.Email,token= resetPasswordToken }, "https", "localhost:44309");	
					await _emailSender.SendAsync(_configs["EmailSettings:SenderEmail"],model.Email,"Reset Password",resetPasswordUrl);
				}
				ModelState.AddModelError(string.Empty, "There is no account with this email");
				return RedirectToAction(nameof(CheckYourInbox));
			}
			return View(model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ResetPassword(string email,string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid) 
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;
				var user = await _UserManager.FindByEmailAsync(email);
				if(user is not null)
				{

				await _UserManager.ResetPasswordAsync(user, token, model.Password);
				return RedirectToAction(nameof(SignIn));
				}
				ModelState.AddModelError(string.Empty, "Url is not valid");
			}
			return View(model);
		}
	}
}
