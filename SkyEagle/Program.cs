using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkyEagle.Classes;
using SkyEagle.Repositories.Implementations;
using SkyEagle.Repositories.Interfaces;
using SkyModel;
using System;
using System.Net;

namespace SkyEagle;

public class Program
{
	public static void Main(string[] _)
	{
		WebApplicationBuilder builder = WebApplication.CreateSlimBuilder();
		builder.Logging.SetMinimumLevel(LogLevel.None);
		builder.Services.AddControllers();
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		builder.WebHost.UseKestrel(option =>
		{
			option.AddServerHeader = false;
			option.Limits.MaxRequestBodySize = 10000000; // 10mb
			option.Listen(IPAddress.Any, 5042, listenOptions =>
			{
				listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2AndHttp3;
				listenOptions.UseHttps();
			});
		});
		builder.Services.AddTransient<SkyDbContext>();
		builder.Services.AddPooledDbContextFactory<SkyDbContext>(options =>
		{
			options.UseLazyLoadingProxies().UseSqlServer(SkyDbContext.ConnectionString, x => x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
			options.AddInterceptors(SkyDbContext.NoLock);
		});
		builder.Services.AddScoped<IProductRepository, ProductRepository>();
		builder.Services.AddScoped<IMinIORepository, MinIORepository>();

		// Set config cho minIO
		MinIORepository.MinIOAccessKey = builder.Configuration["MinIO:AccessKey"] ?? throw new InvalidOperationException("The MinIO AccessKey is not configured. Please set the 'MinIO:AccessKey' configuration.");
		MinIORepository.MinIOSecretKey = builder.Configuration["MinIO:SecretKey"] ?? throw new InvalidOperationException("The MinIO SecretKey is not configured. Please set the 'MinIO:SecretKey' configuration.");

		WebApplication app = builder.Build();
		INIT.ServiceProvider = app.Services;
		SkyDbContext db = INIT.ServiceProvider.CreateDbContext();
		db.Database.Migrate();
		db.Dispose();

		app.UseHttpsRedirection();
		app.UseHsts();
		app.UseRouting();
		app.MapControllers();
		app.Run();
	}
}