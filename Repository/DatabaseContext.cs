using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository;

public class DatabaseContext : IdentityDbContext<User>
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
            
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfiguration(new CountryConfiguration());	
		modelBuilder.ApplyConfiguration(new HotelConfiguration());	
		modelBuilder.ApplyConfiguration(new RoleConfiguration());	
	}

	public DbSet<Country>? Countries { get; set; }   
    public DbSet<Hotel>? Hotels { get; set; }
}
