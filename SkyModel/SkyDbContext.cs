using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using SkyModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkyModel;

public class SkyDbContext : DbContext
{
	//public SkyDbContext() { }

	public SkyDbContext(DbContextOptions<SkyDbContext> optionsBuilder) : base(optionsBuilder) { }

	//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//{
	//	optionsBuilder.UseLazyLoadingProxies().UseSqlServer(ConnectionString);
	//	optionsBuilder.AddInterceptors(NoLock);
	//}

	// DEBUG / RELEASE
#if DEBUG
	public const string ConnectionString = $"";
#else
	public const string ConnectionString = $"";
#endif
	public static readonly NoLockInterceptor NoLock = new();
	public string? Error;

	protected override void OnModelCreating(ModelBuilder modelBuilder) // chỉ khi composite keys
	{
		foreach (IMutableForeignKey foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			foreignKey.DeleteBehavior = DeleteBehavior.ClientCascade;
	}

	private void ValidateEntities()
	{
		List<ValidationResult> validationResults = new();
		IEnumerable<object> entities = from entry in ChangeTracker.Entries()
									   where entry.State == EntityState.Added || entry.State == EntityState.Modified
									   select entry.Entity;
		foreach (object entity in entities)
			if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true))
			{
				Error = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
				Console.WriteLine(Error);
				throw new ValidationException(Error);
			}
	}

	public override int SaveChanges()
	{
		ValidateEntities();
		return base.SaveChanges();
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		ValidateEntities();
		return await base.SaveChangesAsync(cancellationToken);
	}

	public DbSet<Realpty> Realptys { get; set; }
}

//public class MyMigration : IDesignTimeDbContextFactory<SkyDbContext> // for code first
//{
//	public SkyDbContext CreateDbContext(string[] _) => new();
//}