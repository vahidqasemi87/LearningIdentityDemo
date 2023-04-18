using IdenDemo.Models;

namespace IdenDemo.Services;

public interface IUserService
{
	Task<string> RegisterAsync(RegisterModel model);

}
