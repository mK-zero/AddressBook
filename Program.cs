using AddressBook.Data;
using AddressBook.Services;
using AddressBook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(DataUtility.GetConnectionString(builder.Configuration)));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddScoped<IImageService, BasicImageService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider
                            .GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

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
