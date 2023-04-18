using IdenDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace IdenDemo.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWT _jWT;
    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JWT jWT)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jWT = jWT;
    }

	public async Task<string> RegisterAsync(RegisterModel model)
	{
        var user =
            new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

		var userWithSameEmail =
            await _userManager.FindByEmailAsync(model.Email);



		if (userWithSameEmail == null)
        {
			var result =
                await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, Authorization.default_role.ToString());
			}
			return $"User Registered with username {user.UserName}";

		}
		else
		{
			return $"Email {user.Email} is already registered.";
		}
	}
}
