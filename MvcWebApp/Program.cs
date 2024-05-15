using AzureStorageLibrary;
using AzureStorageLibrary.Interfaces;
using AzureStorageLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConnectionStrings.AzureStorageConnectionString = builder.Configuration.GetSection("AzureConnectionStrings")["AzureStorageStr"];

builder.Services.AddScoped(typeof(INoSqlStorage<>), typeof(TableStorage<>));
builder.Services.AddControllersWithViews();


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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();