using Microsoft.AspNetCore.Authentication.Cookies;
using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebBriliFresh.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WebBriliFresh.Repositories.Abstract;
using WebBriliFresh.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<BriliFreshDbContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("BriliFreshDB")
));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// For Identity  
builder.Services.AddIdentity<User, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<BriliFreshDbContext>();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/UserLogin/Login");

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "ADMIN"));
    options.AddPolicy("Employee", policy => policy.RequireClaim(ClaimTypes.Role, "EMPLOYEE", "ADMIN"));
    options.AddPolicy("LoggedIn", policy => policy.RequireClaim(ClaimTypes.Role, "ADMIN", "CUSTOMER", "EMPLOYEE"));


});


builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(),"Uploads")
//        ),
//    RequestPath = "/contents"
//});

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
