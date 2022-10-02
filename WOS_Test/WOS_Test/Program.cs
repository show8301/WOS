using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WOS_Test.Models;
using WOS_Test.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<WOSContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WOSDatabase")));

builder.Services.AddScoped<UserDatumService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();



