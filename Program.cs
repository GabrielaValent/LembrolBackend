using backend_lembrol.Database;
using backend_lembrol.Service;
using backend_lembrol.Repository;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataAccess;
using backend_lembrol.DataAccess.Interfaces;
using backend_lembrol.MQTT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add MQTT services
builder.Services.AddSingleton<MqttService>();

// Register the factory for TagRepository
builder.Services.AddSingleton<TagRepositoryFactory>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add repositories and services
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var allowedCors = builder.Configuration["ALLOWED_CORS"];

if (!string.IsNullOrEmpty(allowedCors))
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecific", policyBuilder =>
        {
            policyBuilder.WithOrigins(allowedCors)
                         .AllowAnyMethod()
                         .AllowAnyHeader();
        });
    });
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}
else
{
    app.UseCors("AllowSpecific");
}

app.Services.GetService<MqttService>();

app.Run();
