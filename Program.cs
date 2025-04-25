using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskTrackerDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// Get connection string with fallback
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=TaskTracker.db";

// Single DbContext configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    .LogTo(Console.WriteLine, LogLevel.Information) // Add logging
    .EnableSensitiveDataLogging());

// Identity configuration
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// JSON configuration
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
    });
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LogoutPath = "/Identity/Account/Logout";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy => 
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "IsManager" && c.Value == "true")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Create Manager Role
        if (!await roleManager.RoleExistsAsync("Manager"))
        {
            await roleManager.CreateAsync(new IdentityRole("Manager"));
        }

        // === Create Admin Role ===
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Create Member Role
        if (!await roleManager.RoleExistsAsync("Member"))
        {
            await roleManager.CreateAsync(new IdentityRole("Member"));
        }


        // === Create Admin User ===
        var adminEmail = "admin@niyo.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                FullName = "Super Admin",
                Email = adminEmail,
                UserName = adminEmail,
                IsManager = true, // because admin is also treated like a manager
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@1234");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(adminUser, "Manager");
                Console.WriteLine("Admin user created successfully");
            }
            else
            {
                Console.WriteLine("Admin creation failed:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        else
        {
            // ✅ If user already exists, make sure both roles are set
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                await userManager.AddToRoleAsync(adminUser, "Admin");

            if (!await userManager.IsInRoleAsync(adminUser, "Manager"))
                await userManager.AddToRoleAsync(adminUser, "Manager");

            // ✅ Update IsManager flag if missing
            if (!adminUser.IsManager)
            {
                adminUser.IsManager = true;
                await userManager.UpdateAsync(adminUser);
            }
        }
    }


    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding data: {ex.Message}");
    }
}

app.Run();