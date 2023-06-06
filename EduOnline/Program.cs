using EduOnline.DAL;
using EduOnline.DAL.Entities;
using EduOnline.Helpers;
using EduOnline.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region dependencies
builder.Services.AddDbContext<DatabaseContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(io =>
{
    io.User.RequireUniqueEmail = true;
    io.Password.RequireDigit = false;
    io.Password.RequiredUniqueChars = 0;
    io.Password.RequireLowercase = false;
    io.Password.RequireNonAlphanumeric = false;
    io.Password.RequireUppercase = false;
    io.Password.RequiredLength = 6;

}).AddEntityFrameworkStores<DatabaseContext>();

#endregion

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Unauthorized";
});

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IDropDownListsHelper, DropDownListsHelper>();
builder.Services.AddScoped<IAzureBlobHelper, AzureBlobHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
