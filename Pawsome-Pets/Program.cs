using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<PawsomeDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<PawsomeDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Roles seeding
using (IServiceScope scope = app.Services.CreateScope())
{
	RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	string[] roles = new[] { "Admin", "Giver", "Adopter", "Caretaker" };

	foreach (string role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}

	//Admin seed

	//string admin = 

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
