using Kernel.DependencyInjection;
using TemperatureMeter.Application.DependencyInjection;
using TemperatureMeterApi.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AddApiInstallers(builder);
AddApplicationInstallers(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

static void AddApiInstallers(WebApplicationBuilder builder)
{
    builder.Services.AddInstallersFromAssemblyContaining<ITemperatureMeterApiAssemblyMarker>(builder.Configuration);
}

static void AddApplicationInstallers(WebApplicationBuilder builder)
{
    builder.Services.AddInstallersFromAssemblyContaining<ITemperatureMeterApplicationMarker>(builder.Configuration);
}