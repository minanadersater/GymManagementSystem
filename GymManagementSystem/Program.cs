using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Context;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Utilities;
using GymManagementSystem;
using Microsoft.AspNetCore.Identity;
using GymManagementSystem.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GymDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddScoped<IPlanRepository, PlanRepository>();

//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMemberServices, MemberServices>();
builder.Services.AddScoped<ISessionServices, SessionServices>();
builder.Services.AddScoped<ITrainerServices, TrainerServices>();
builder.Services.AddScoped<IAttachementServices, AttachementServices>();
builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>)
);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //options.Password.RequireLength = 6;
    //options.Password.RequireLowercase = true;
    //options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = false;
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
}).AddEntityFrameworkStores<GymDbcontext>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

});

builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfile()));

var app = builder.Build();
await app.MigrateAndSeedAsync();

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


app.Run();
