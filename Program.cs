using Microsoft.EntityFrameworkCore;
using PetShopProj.Data;
using PetShopProj.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Repository>();
//builder.Services.AddDbContext<CityContext>(options => options.UseSqlite("Data Source=c:\\temp\\exercise.db"));
//string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=aspnet-amir1;Integrated Security=True";
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<PetDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<PetDbContext>();
    ctx.Database.EnsureDeleted();
    ctx.Database.EnsureCreated();
}

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("Default", "{controller=DbData}/{action=Index}/{id?}");
});

app.Run();
