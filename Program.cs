using ChatRooms.Data;
using ChatRooms.Helpers;
using ChatRooms.Hubs;
using ChatRooms.Interfaces;
using ChatRooms.Models;
using ChatRooms.Repository;
using ChatRooms.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatroomRepository, ChatroomRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IChatroomService, ChatroomService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddDbContext<ChatroomContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatRoomsContextDev"));
});
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ChatroomContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Serilog logger to log to console and text file
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning)
    .WriteTo.File(path: "Logs/Error-log-.txt", restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
    .WriteTo.File(path: "Logs/Chat-log-.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

// Create the database and seed it
DbInitializer.CreateDatabase(app);
await DbInitializer.SeedUsersAndRolesAsync(app);
DbInitializer.Initialize(app);

// Call database creation and seeding through console
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    DbInitializer.CreateDatabase(app);
    await DbInitializer.SeedUsersAndRolesAsync(app);
    DbInitializer.Initialize(app);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR mapping the ChatHub
app.MapHub<ChatHub>("/chatHub");

app.Run();
