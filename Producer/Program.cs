using Producer.IOC;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region Services
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
