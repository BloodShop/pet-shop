using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetShopProj.Data;
using PetShopProj.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IRepository, PetRepository>();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<PetDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
}).AddEntityFrameworkStores<PetDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);
builder.Services.ConfigureApplicationCookie(config => config.LoginPath = "/Login");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<PetDbContext>();
    var userMngr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleMngr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    //ctx.Database.EnsureDeleted(); // remove
    ctx.Database.EnsureCreated();

    var adminRole = new IdentityRole("Admin");
    if (!ctx.Roles.Any())
    {
        // Create role
        roleMngr.CreateAsync(adminRole).GetAwaiter().GetResult();
    }

    if (!ctx.Users.Any(u => u.UserName == "Admin"))
    {
        var adminUser = new IdentityUser
        {
            UserName = "Admin",
            Email = "admin@test.com",
        };
        var result = userMngr.CreateAsync(adminUser, "123456aA").GetAwaiter().GetResult();
        // add role to user
        userMngr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
    }
}

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{Action=Index}/{id?}"));
app.Run();