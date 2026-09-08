using Microsoft.EntityFrameworkCore;
using ApiSampleForSDLC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the EmployeeContext – using the InMemory provider so no external DB is required.
builder.Services.AddDbContext<EmployeeContext>(options =>
    options.UseInMemoryDatabase("EmployeeDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
