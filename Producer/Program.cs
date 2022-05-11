using Producer.IOC;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region Services
services.AddControllers();
#region CAP
builder.Services.AddCap(x =>
{
	x.UseInMemoryMessageQueue();
	x.UseInMemoryStorage();
	//todo: add to config
	x.UseKafka(builder.Configuration.GetSection("KafkaConfigs")["Bootstrapserver"]);
});
#endregion
DependencyContainer.RegisterServices(services);
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
