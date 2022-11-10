using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetShopProj.Data;
using PetShopProj.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IRepository, Repository>();
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<PetDbContext>();
    var userMngr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleMngr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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

    //ctx.Database.EnsureDeleted();
    //ctx.Database.EnsureCreated();
}

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
        app.UseMigrationsEndPoint();
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
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.Run();