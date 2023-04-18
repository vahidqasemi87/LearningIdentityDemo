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
}
