using Producer.IOC;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region Services
#region Logger
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Warn("SentMessages\tTransferRate");
//builder.Logging.AddLog4Net("log4net.config");
builder.Host.UseNLog();
#endregion
services.AddControllers();
DependencyContainer.RegisterServices(services, builder.Configuration);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
#endregion


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
