using RabbitMQImplementation.Repository.Consumers;
using RabbitMQImplementation.Repository.Producers.IServices;
using RabbitMQImplementation.Repository.Producers.ServicesManager;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

// Add services to the container.

builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddTransient<IHostedService, UserWorker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();