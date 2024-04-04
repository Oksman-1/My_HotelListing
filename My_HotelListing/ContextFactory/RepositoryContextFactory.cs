using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace My_HotelListing.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
	public DatabaseContext CreateDbContext(string[] args)
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		var builder = new DbContextOptionsBuilder<DatabaseContext>()
			.UseSqlServer(configuration.GetConnectionString("SQLConnectionString"),
			   b => b.MigrationsAssembly("My_HotelListing"));

		return new DatabaseContext(builder.Options);
	}
}
