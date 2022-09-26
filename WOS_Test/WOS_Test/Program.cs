using Microsoft.EntityFrameworkCore;
using WOS_Test.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<WOSContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WOSDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



