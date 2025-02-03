using FitnessPlanner.BL.Services;
using FitnessPlanner.DL.Repositories;
using FitnessPlanner.Models.Configurations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection("MongoDbSettings"));


builder.Services.AddSingleton<IWorkoutRepository, WorkoutRepository>();


builder.Services.AddSingleton<IWorkoutService, WorkoutService>();


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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