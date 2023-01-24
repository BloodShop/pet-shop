using Microsoft.AspNetCore.Identity;
using PetShopProj.Configuration;
using PetShopProj.Data;
using PetShopProj.Hubs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
/*builder.WebHost.UseKestrel()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseIISIntegration()
    .UseUrls("http://localhost:8080/");*/
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
builder.Services.InstallerServices(
    builder.Configuration,
    typeof(IServiceInstaller).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<PetDbContext>();
    var userMngr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleMngr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    /* ctx.Database.EnsureDeleted(); // remove */
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
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseCors(builder => // Allows to get inforamation fron any api
{
    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<CallCenterHub>("/callcenter");
    endpoints.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{Action=Index}/{id?}");

});
app.Run();