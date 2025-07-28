using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Services.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<PawsomeDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<PawsomeDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/Login";
	options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAdoptionRequestService, AdoptionRequestService>();


var app = builder.Build();

// Roles seeding
using (IServiceScope scope = app.Services.CreateScope())
{
	RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
	string[] roles = new[] { "Admin", "Giver", "Adopter", "Caretaker" };

	foreach (string role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}

	//Admin seed

	string adminEmail = "admin@pawsomepets.com";
	string adminPassword = "Admin-12345";

	ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

	if (adminUser == null)
	{
		adminUser = new ApplicationUser()
		{
			UserName = "Admin",
			Email = adminEmail,
			FirstName = "Admin",
			LastName = "Admin",
			EmailConfirmed = true
		};
		IdentityResult createAdmin = await userManager.CreateAsync(adminUser, adminPassword);

		if (createAdmin.Succeeded)
		{
			await userManager.AddToRoleAsync(adminUser, "Admin");
		}
		else
		{
			foreach (var error in createAdmin.Errors)
			{
				Console.WriteLine($"Error creating admin user: {error.Description}");
			}
		}

	}

	// Middleware pipeline
	if (app.Environment.IsDevelopment())
	{
		app.UseMigrationsEndPoint();
	}
	else
	{
		app.UseExceptionHandler("/Home/Error");
		app.UseHsts();
	}
	app.UseSession();
	app.UseHttpsRedirection();
	app.UseStaticFiles();
	app.UseRouting();
	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
	app.MapRazorPages();

	app.Run();
}
