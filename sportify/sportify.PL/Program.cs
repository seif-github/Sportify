using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sportify.DAL.Repositories.Contracts;
using sportify.DAL.Repositories;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.BLL.Services.Contracts;
using sportify.BLL.Services;
using Microsoft.AspNetCore.Http.Features;
using sportify.BLL.Settings;
using Microsoft.Extensions.Options;
using sportify.PL.Hubs;
using sportify.PL.Helpers;
using DinkToPdf.Contracts;
using DinkToPdf;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<SportifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs")),
    ServiceLifetime.Scoped);

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("gmail"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
{
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true; 
})
    .AddEntityFrameworkStores<SportifyContext>()
    .AddDefaultTokenProviders();

#region Cookie Settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(4);
    options.SlidingExpiration = true;
}
            );
#endregion

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<ILeagueTeamCountUpdateService, LeagueTeamCountUpdateService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB file size limit
});

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


builder.Services.AddSignalR();
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

app.MapHub<ScoreHub>("/scoreHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
