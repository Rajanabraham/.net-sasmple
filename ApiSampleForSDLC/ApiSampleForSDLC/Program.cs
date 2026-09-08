using ApiSampleForSDLC.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext. Uses a SQL Server connection string defined in appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("EmployeeDatabase");
if (string.IsNullOrWhiteSpace(connectionString))
{
    // Fallback to InMemory for local development / tests.
    builder.Services.AddDbContext<EmployeeContext>(options =>
        options.UseInMemoryDatabase("EmployeeDb"));
}
else
{
    builder.Services.AddDbContext<EmployeeContext>(options =>
        options.UseSqlServer(connectionString));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
