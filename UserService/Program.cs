using HealthChecks.SqlServer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UserService;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

//Db Context
builder.Services.AddDbContext<UserServiceDbContext>(options =>
    options.UseSqlServer(connectionString));

//Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("Db-check",
        new SqlConnectionHealthCheck(connectionString),
        HealthStatus.Unhealthy,
        new string[] { "db", "sql", "check" })
    .AddCheck<UserServiceHealthCheck>("User Service Check");

//Health Check UI
builder.Services.AddHealthChecksUI(setup => setup.SetEvaluationTimeInSeconds(5))
    .AddSqlServerStorage(connectionString);

var app = builder.Build();

app.MapHealthChecksUI(config => config.UIPath = "/hc-ui");
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();