using Consumer.IOC;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLog4Net("log4net.config");
var services = builder.Services;
#region Services
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
DependencyContainer.RegisterServices(services, builder.Configuration);
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
