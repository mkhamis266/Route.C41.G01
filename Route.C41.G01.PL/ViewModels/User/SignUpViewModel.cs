using System.ComponentModel.DataAnnotations;

namespace Route.C41.G01.PL.ViewModels.User
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]	
		public string Email { get; set; }

		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[Required(ErrorMessage ="First Name is required")]
		public string FirstName { get; set; }


		[Required(ErrorMessage = "Last Name is required")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[MinLength(5,ErrorMessage ="Minimum Password is 5 ")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[MinLength(5, ErrorMessage = "Minimum Password is 5 ")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password),ErrorMessage = "Password dosn't Match")]
		public string ConfirmPassword { get; set; }
		public bool IsAgree {  get; set; }
	}
}
