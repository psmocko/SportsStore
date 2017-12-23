using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = BuildWebHost(args);

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        try
        {

           //SeedData.SeedDatabase(services.GetRequiredService<DataContext>());
           //IdentitySeedData.SeedDatabase(services.GetRequiredService<IdentityDataContext>(), services).Wait();
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred seeding the DB.");
        }
      }

      host.Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .ConfigureAppConfiguration((hostContext, config) =>
            {
              config.Sources.Clear();
              config.AddJsonFile("appsettings.json", optional: true);
            })
            .Build();

    //public static IWebHost BuildWebHost(string[] args) {
    //  return new WebHostBuilder()
    //            .UseKestrel()
    //            .UseContentRoot(Directory.GetCurrentDirectory())
    //            .ConfigureAppConfiguration((hostingContext, config) =>
    //            {
    //              var env = hostingContext.HostingEnvironment;

    //              config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    //                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

    //              if (env.IsDevelopment())
    //              {
    //                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
    //                if (appAssembly != null)
    //                {
    //                  config.AddUserSecrets(appAssembly, optional: true);
    //                }
    //              }

    //              config.AddEnvironmentVariables();

    //              if (args != null)
    //              {
    //                config.AddCommandLine(args);
    //              }
    //            })
    //            .ConfigureLogging((hostingContext, logging) =>
    //            {
    //              logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
    //              logging.AddConsole();
    //              logging.AddDebug();
    //            })
    //            //.UseIISIntegration()
    //            .UseDefaultServiceProvider((context, options) =>
    //            {
    //              options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    //            })
    //            .UseStartup<Startup>()
    //            .Build();
    //}

  }

  
}
