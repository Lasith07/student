using DemoAPI;
using Microsoft.EntityFrameworkCore;
using DemoAPI.Services.StudentService;
using DemoAPI.Services.SubjectService;
using DemoAPI.Services.UserService;
using DemoAPI.Helpers.Utils;
using DemoAPI.Middlewares;
using DemoAPI.Helpers.Utils.GlobalAttributes;
using DemoAPI.Services.StoryService;

var builder = WebApplication.CreateBuilder(args);

       

GlobalAttributes.MySqlConfiguration.connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});

builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllers();
builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseJwtMiddleware();  

app.UseAuthorization();

app.MapControllers();

app.Run();
