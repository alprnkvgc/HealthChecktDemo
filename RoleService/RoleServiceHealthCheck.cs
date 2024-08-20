using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RoleService;

public class RoleServiceHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;

    public RoleServiceHealthCheck(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            var response = await _httpClient.GetAsync("https://localhost:7238/WeatherForecast", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("WeatherForecastController is running fine.");
            }
            else
            {
                return HealthCheckResult.Unhealthy("WeatherForecastController is not responding correctly.");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("WeatherForecastController check failed.", ex);
        }
    }
}