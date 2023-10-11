using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Configurations;

namespace Repository;
public class RepositoryContext: DbContext
{
    public RepositoryContext(DbContextOptions options): base(options)   
    {
        
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

        modelBuilder.Entity<Company>()
            .HasMany(c => c.Employees)
            .WithOne(c => c.Company)
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
    
	public DbSet<Employee>? Employees { get; set; }
    public DbSet<Company>? Companies { get; set; }

}
