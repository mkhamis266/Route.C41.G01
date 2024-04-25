using System.ComponentModel.DataAnnotations;

namespace Route.C41.G01.PL.ViewModels.Account
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is required")]
		[MinLength(5, ErrorMessage = "Minimum Password is 5 ")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[MinLength(5, ErrorMessage = "Minimum Password is 5 ")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password dosn't Match")]
		public string ConfirmPassword { get; set; }

		public string Email { get; set; }
		public string Token { get; set; }
	}
}
