using Consumer.IOC;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Warn("InputRate\tMissRate");
//builder.Logging.AddLog4Net("log4net.config");
builder.Host.UseNLog();
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
