
using IdenDemo.Models;
using IdenDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdenDemo;

public class Program
{
	public async static Task Main(string[] args)
	{
		var builder =
			WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();



		#region [Configure Identity]
		//Configuration from AppSettings
		builder.Services.Configure<JWT>(config: builder.Configuration.GetSection(key: "JWT"));

		//User Manager Service
		builder
			.Services
			.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

		builder.Services.AddScoped<IUserService, UserService>();


		//Adding DB Context with MSSQL
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(
				builder.Configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


		//Adding Athentication - JWT
		builder.Services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(o =>
			 {
				 o.RequireHttpsMetadata = false;
				 o.SaveToken = false;
				 o.TokenValidationParameters = new TokenValidationParameters
				 {
					 ValidateIssuerSigningKey = true,
					 ValidateIssuer = true,
					 ValidateAudience = true,
					 ValidateLifetime = true,
					 ClockSkew = TimeSpan.Zero,
					 ValidIssuer = builder.Configuration["JWT:Issuer"],
					 ValidAudience = builder.Configuration["JWT:Audience"],
					 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
				 };
			 });

		builder.Services.AddControllers();

		#endregion


		var app = builder
			.Build();


		using (var scope = app.Services.CreateAsyncScope())
		{
			var services = scope.ServiceProvider;

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				//Seed Default Users
				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				await ApplicationDbContextSeed.SeedEssentialsAsync(userManager, roleManager);
			}
			catch (Exception ex)
			{

				var logger = loggerFactory.CreateLogger<Program>();

				logger
					.LogError(exception: ex, message: "An error occurred seeding the DB.");
			}
		}

		// Configure the HTTP request pipeline.

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseDeveloperExceptionPage();
		}

		app.UseHttpsRedirection();

		app.UseRouting();

		app.UseAuthentication();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}