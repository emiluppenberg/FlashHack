
using FlashHack.Data;
using FlashHack.Data.DataInterfaces;
using FlashHack.Data.MockData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();

var app = builder.Build();

//OBS! R�R EJ
//MOCK DATA 
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    DbInitializer.Initialize(services);
//}
//OBS! R�R EJ

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
