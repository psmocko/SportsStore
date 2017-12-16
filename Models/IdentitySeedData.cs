using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
  public class IdentitySeedData
  {
    private const string adminUser = "admin";
    private const string adminPassword = "MySecret123$";
    private const string adminRole = "Administrator";

    private readonly IdentityDataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public IdentitySeedData(IdentityDataContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)  
    {
      _context = context;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task SeedDatabase() {
      _context.Database.Migrate();

      var role = await _roleManager.FindByNameAsync(adminRole);
      var user = await _userManager.FindByNameAsync(adminUser);

      if (role == null) {
        role = new IdentityRole(adminRole);
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded) {
          throw new Exception($"Cannot create role: {result.Errors.FirstOrDefault()}");
        }
      }

      if (user == null) {
        user = new IdentityUser(adminUser);
        var result = await _userManager.CreateAsync(user, adminPassword);
        if (!result.Succeeded) {
          throw new Exception($"Cannot create user: {result.Errors.FirstOrDefault()}");
        }
      }

      if (!await _userManager.IsInRoleAsync(user, adminRole)) {
        var result = await _userManager.AddToRoleAsync(user, adminRole);
        if (!result.Succeeded) {
          throw new Exception($"Cannot add user to role: {result.Errors.FirstOrDefault()}");
        }
      }      
    }

    public async Task SeedDatabase(IApplicationBuilder app) {
      (GetAppService<IdentityDataContext>(app)).Database.Migrate();

      var userManager = GetAppService<UserManager<IdentityUser>>(app);
      var roleManager = GetAppService<RoleManager<IdentityRole>>(app);

      var role = await roleManager.FindByNameAsync(adminRole);
      var user = await userManager.FindByNameAsync(adminUser);

      if (role == null)
      {
        role = new IdentityRole(adminRole);
        var result = await roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
          throw new Exception($"Cannot create role: {result.Errors.FirstOrDefault()}");
        }
      }

      if (user == null)
      {
        user = new IdentityUser(adminUser);
        var result = await userManager.CreateAsync(user, adminPassword);
        if (!result.Succeeded)
        {
          throw new Exception($"Cannot create user: {result.Errors.FirstOrDefault()}");
        }
      }

      if (!await userManager.IsInRoleAsync(user, adminRole))
      {
        var result = await userManager.AddToRoleAsync(user, adminRole);
        if (!result.Succeeded)
        {
          throw new Exception($"Cannot add user to role: {result.Errors.FirstOrDefault()}");
        }
      }
    }

    private static T GetAppService<T>(IApplicationBuilder app)
    {
      return app.ApplicationServices.GetRequiredService<T>();
    }
  }
}
