using System;
using SchoolDatabase.Models;
using SchoolDatabase.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Swagger API help pages
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddScoped<SchoolDbContext>();
// API for now, but should be added as separate Teacher Service
builder.Services.AddScoped<TeacherAPIController>();
// API for now, but should be added as separate student Service
builder.Services.AddScoped<StudentAPIController>();
// API for now, but should be added as separate Course Service
builder.Services.AddScoped<CourseAPIController>();


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

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
