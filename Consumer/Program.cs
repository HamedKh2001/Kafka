using Consumer.IOC;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
#region Services
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
DependencyContainer.RegisterServices(services);
#region CAP
services.AddCap(x =>
{
	x.UseInMemoryMessageQueue();
	x.UseInMemoryStorage();
	x.UseKafka("localhost:9092");
});
#endregion
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
