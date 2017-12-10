using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Newtonsoft.Json;
using System;

namespace SportsStore
{
  public class Startup
  {

    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

      Configuration = builder.Build();

    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Products")));
      services.AddMvc()
        .AddJsonOptions(opts =>
        {
          opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
          opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

        });

      services.AddDistributedSqlServerCache(options =>
      {
        options.ConnectionString = Configuration.GetConnectionString("Products");
        options.SchemaName = "dbo";
        options.TableName = "SessionData";
      });

      services.AddSession(options => {
        options.Cookie.Name = "SportStore.Session";
        options.Cookie.HttpOnly = false;
        options.IdleTimeout = TimeSpan.FromHours(48);        
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {

      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseDeveloperExceptionPage();
      app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
      {
        HotModuleReplacement = true,
        HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
      });

      app.UseStaticFiles();
      app.UseSession();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");

          routes.MapSpaFallbackRoute("angular-fallback", new { controller = "Home", action = "Index" });
      });

      // SeedData.SeedDatabase(app.ApplicationServices.GetRequiredService<DataContext>());
    }
  }
}
