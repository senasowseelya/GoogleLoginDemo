
using GoogleLoginDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleLoginDemo.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly AuthenticationService _authService;
		public UserController(AuthenticationService authService)
		{
			_authService = authService;
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Login(string idToken)
		{
			return Ok(_authService.Authenticate(idToken));
		}

	}
}
