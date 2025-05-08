using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sportify.DAL.Repositories.Contracts;
using sportify.DAL.Repositories;
using sportify.DAL.Data;
using sportify.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<SportifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"))); 


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SportifyContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(MapperProfile));


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication(); 
app.UseAuthorization(); 


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
