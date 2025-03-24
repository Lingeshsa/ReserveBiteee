using Microsoft.AspNetCore.Identity;
using ReserveBiteee.DataBaseHelper;
using ReserveBiteee.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .WriteTo.File("Log/Log.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

// Register Services
builder.Services.AddScoped<DatabaseHelper>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddScoped<ICustomerOrderService, CustomerOrderService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IReservationService, ReservationService>();


builder.Services.AddScoped<IEmailService, EmailService>();





// Configure Authorization Policies
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
//});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
