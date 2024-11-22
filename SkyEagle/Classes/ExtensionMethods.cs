using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkyModel;
using System;

namespace SkyEagle.Classes;

internal static class ExtensionMethods
{
	internal static SkyDbContext CreateDbContext(this IServiceProvider service)
	{
		IDbContextFactory<SkyDbContext> fac = service.GetRequiredService<IDbContextFactory<SkyDbContext>>();
		return fac.CreateDbContext();
	}
}