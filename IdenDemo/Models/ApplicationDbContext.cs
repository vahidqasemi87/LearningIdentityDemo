using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdenDemo.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}
	//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//{
	//	base.OnConfiguring(optionsBuilder);
	//}
	//protected override void OnModelCreating(ModelBuilder builder)
	//{
	//	builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
	//}
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	//private readonly IConfiguration _configuration;
 //   public ApplicationDbContextFactory(IConfiguration configuration)
 //   {
	//	_configuration = configuration;
 //   }
    //public ApplicationDbContextFactory()
    //{
        
    //}
    public ApplicationDbContext CreateDbContext(string[] args)
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		//builder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
		builder.UseSqlServer("server=.;database=IdentityDb;uid=sa;pwd=Ss12345!@#$%;trustservercertificate=true;");
		return new ApplicationDbContext(builder.Options);
	}
}