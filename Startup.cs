using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
        .AddEnvironmentVariables()
        .AddCommandLine(Environment.GetCommandLineArgs()
          .Skip(1).ToArray());

      Configuration = builder.Build();

    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<IdentityDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Identity")));
      services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDataContext>().AddDefaultTokenProviders();

      services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Products")));
      services.AddMvc()
        .AddJsonOptions(opts =>
        {
          opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
          opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

        });

      services.AddScoped<IdentitySeedData>();

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

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.Events.OnRedirectToLogin = (context) =>
          {
            if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
            {
              context.Response.StatusCode = 401;
             
            }
            else {
              context.Response.Redirect(context.RedirectUri);
            }
            return Task.FromResult<object>(null);
          };
        });

      services.AddAntiforgery(options => {
        options.HeaderName = "X-XSRF-TOKEN";
      });

      return services.BuildServiceProvider();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IdentitySeedData seedData, IAntiforgery antiForgery, IServiceProvider serviceProvider)
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
      app.UseAuthentication();
   
      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");

          routes.MapSpaFallbackRoute("angular-fallback", new { controller = "Home", action = "Index" });
      });

      //if ((Configuration["INITDB"] ?? "false") == "true") {
      //  Console.WriteLine("Preparing Database...");
        SeedData.SeedDatabase(serviceProvider.GetRequiredService<DataContext>());
        await seedData.SeedDatabase(app);
        //await seedData.SeedDatabase();
      //  Console.WriteLine("Database Preparation Complete");
      //  Environment.Exit(0);
      //}

      app.Use(async (context, next) => {
        string path =  context.Request.Path;
        if (context.Request.Path.StartsWithSegments("/api") || context.Request.Path.StartsWithSegments("/"))
        {
          // XSRF-TOKEN used by angular in the $http if provided
          var tokens = antiForgery.GetAndStoreTokens(context);
          context.Response.Cookies.Append("XSRF-TOKEN", antiForgery.GetAndStoreTokens(context).RequestToken);
            //tokens.RequestToken, new CookieOptions
            //{
            //  HttpOnly = false,
            //  Secure = true
            //}
          //);
        }

        await next();
      });

      //SeedData.SeedDatabase(app.ApplicationServices.GetRequiredService<DataContext>());
      
    }
  }

  public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDataContext>
  {
    public IdentityDataContext CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
      var builder = new DbContextOptionsBuilder<IdentityDataContext>();
      var connectionString = configuration.GetConnectionString("Identity");
      builder.UseSqlServer(connectionString);
      return new IdentityDataContext(builder.Options);
    }
  }
}
