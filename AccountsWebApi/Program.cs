using AccountsWebApi.Models;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();
// Add services to the container.
//hi aneeq i am umair khan
// i Mladjflajfda dlkf
//sdfassdff
//ehjdfbsfbaf sdjff adaf d
// my change
//changes made
//pro chamngesmsdhvz da bdvf fsehydf s dgfc
//another change made asldlf fdgfdgf
//qwkjdbhajvsajdashbdj hello
builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddCustomAuthentication();

var dbHost = "127.0.0.1";
var dbName = "userstestdb";
var dbPassword = "Mysql123$";
string connectionStringInitial = $"server={dbHost}; port=3306; database={dbName}; user=root; password={dbPassword};";
    //var connectionstring = $"Server={dbhost};Database={dbname};Trusted_Connection=True;TrustServerCertificate=True;";
    //builder.Services.AddDbContext<UserDbContext>(opt => opt.UseMySql(connectionstring));
string? connectionString = connectionStringInitial.ToString();
builder.Services.AddDbContext<UserDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
