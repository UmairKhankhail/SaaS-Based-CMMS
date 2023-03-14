using AccountsWebApi.Models;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();
// Add services to the container.
//hi aneeq i am umair khan
// i Mladjflajfda dlkf
//sdfassdff
//ehjdfbsfbaf sdjff adaf d
// my change
//changes made
//another change made asldlf fdgfdgf
//qwkjdbhajvsajdashbdj hello
builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddCustomAuthentication();

var dbhost = "127.0.0.1";
var dbname = "userstestdb";
var dbpassword = "Mysql123$";
string connectionstringinitial = $"server={dbhost}; port=3306; database={dbname}; user=root; password={dbpassword};";
    //var connectionstring = $"Server={dbhost};Database={dbname};Trusted_Connection=True;TrustServerCertificate=True;";
    //builder.Services.AddDbContext<UserDbContext>(opt => opt.UseMySql(connectionstring));
string? connectionString = connectionstringinitial.ToString();
builder.Services.AddDbContext<UserDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
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
