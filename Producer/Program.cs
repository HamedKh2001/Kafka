using Producer.Application;
using Producer.Application.IServices;
using Producer.Application.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region Services
services.AddControllers();
services.AddSingleton<IProducer, Producer.Application.Services.Producer>();
#region CAP
services.AddCap(x =>
{
	x.UseInMemoryMessageQueue();
	x.UseInMemoryStorage();
	x.UseKafka("localhost:9092");
	//x.UseKafka(opt =>
	//{
	//	opt.Servers = "";
	//	opt.CustomHeaders = kafkaResult => new List<KeyValuePair<string, string>>
	//{
	//	new KeyValuePair<string, string>("my.kafka.offset", kafkaResult.Offset.ToString()),
	//	new KeyValuePair<string, string>("my.kafka.partition", kafkaResult.Partition.ToString())
	//};
	//});
});
#endregion

#region Quartz
services.AddSingleton<IJobFactory, SingletonJobFactory>();
services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

services.AddSingleton<MessageFactory>();
services.AddSingleton(new JobSchedule(jobType: typeof(MessageFactory), cronExpression: "* * * ? * *"));
services.AddHostedService<QuartzHostedService>();
#endregion

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
