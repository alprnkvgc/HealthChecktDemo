using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UserService
{
    public class UserServiceHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;

        public UserServiceHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7256/WeatherForecast", cancellationToken);

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
}
