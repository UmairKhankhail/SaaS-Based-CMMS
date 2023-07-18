using JwtAuthenticationManager;
using System.Text.Json;
using RedisCachingService;
using Microsoft.EntityFrameworkCore;

using Microsoft.OpenApi.Models;
using PreventiveMaintenanceWebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddScoped<GoogleCalendarService.GoogleCalendar>();

builder.Services.AddCustomAuthentication();

var dbHost = "127.0.0.1";
var dbName = "preventivemaintenancetestdb";
var dbPassword = "Mysql123$";
string connectionStringInitial = $"server={dbHost}; port=3306; database={dbName}; user=root; password={dbPassword};";
//var connectionstring = $"Server={dbhost};Database={dbname};Trusted_Connection=True;TrustServerCertificate=True;";
//builder.Services.AddDbContext<UserDbContext>(opt => opt.UseMySql(connectionstring));
string? connectionString = connectionStringInitial.ToString();
builder.Services.AddDbContext<PreventiveMaintenanceDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//builder.Services.AddSingleton<RedisCachingService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();