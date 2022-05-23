#region

using System.Net.Http.Headers;
using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Foodbank = Foodbank_Project.Models.External.Foodbank;

#endregion

namespace Foodbank_Project.Services;

public class GiveFoodApiService : BackgroundService
{
    /* A reference to the configuration file. */
    private readonly IConfiguration _config;
    /* A stopwatch that is used to measure the time it takes to get a response from the API. */

    private readonly ApplicationContext _ctx;

    /* Creating a new instance of the HttpClient class. */
    private readonly HttpClient _httpClient = new();

    /* Creating a logger that will be used to log information about the service. */
    private readonly ILogger _logger;

    /* Creating a new instance of the class, and setting the logger and configuration variables which are injected from builder.Services */
    public GiveFoodApiService(ILoggerFactory logger, IConfiguration configuration, IServiceProvider service)
    {
        _logger = logger.CreateLogger("Services.GiveFoodApi");
        _config = configuration.GetSection("Services:GiveFoodApi");
        _ctx = service.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var runAtStart = _config.GetValue<bool>("RunAtStart");
        _logger.LogInformation("Service started. Runs every {Stamp}",
            TimeSpan.FromSeconds(_config.GetValue<int>("Interval")).ToString(@"h\hm\ms\s"));
        _httpClient.Timeout = TimeSpan.FromSeconds(_config.GetValue<int>("Timeout"));
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!runAtStart) await Task.Delay(_config.GetValue<int>("Interval") * 1000, stoppingToken);

            runAtStart = false;

            _logger.LogInformation("Service run started");

            var resultWrapper = await ServiceHelpers.TimeoutTask(_config.GetValue<int>("Timeout") * 1000,
                async token =>
                    await GetFoodbank<List<Foodbank>>("https://www.givefood.org.uk/api/2/foodbanks/",
                        token), stoppingToken);

            switch (resultWrapper.ResultCode)
            {
                case ServiceHelpers.ResultWrapper<List<Foodbank>>.Code.Success:
                {
                    _logger.LogInformation("Successfully got {Count} foodbanks", resultWrapper.Result?.Count);
                    //dev only !!!
                    //resultWrapper.Result.RemoveRange(25, resultWrapper.Result.Count-25);
                    for (var i = 0; i < resultWrapper.Result?.Count; i++)
                    {
                        var resultWrapperInner = await ServiceHelpers.TimeoutTask(
                            _config.GetValue<int>("Timeout") * 1000,
                            async token =>
                                await GetFoodbank<Foodbank>(resultWrapper.Result?[i].Urls?.Self ?? "",
                                    token), stoppingToken);

                        switch (resultWrapperInner.ResultCode)
                        {
                            case ServiceHelpers.ResultWrapper<Foodbank>.Code.Success:
                            {
                                resultWrapper.Result?[i].Merge(resultWrapperInner.Result!);
                                _logger.LogInformation(
                                    "Successfully added locations for {Name}({Self}) [{I} of {Count}]",
                                    resultWrapper.Result?[i].Name, resultWrapper.Result?[i].Urls?.Self, i + 1,
                                    resultWrapper.Result?.Count);

                                var internalFoodbank = FoodbankHelpers.Convert(resultWrapper.Result![i]);

                                internalFoodbank.Status = Status.Approved;

                                await FoodbankHelpers.InsertOrUpdate(internalFoodbank, _ctx, stoppingToken);


                                await _ctx.SaveChangesAsync(stoppingToken);

                                break;
                            }
                            case ServiceHelpers.ResultWrapper<Foodbank>.Code.Timeout:
                            {
                                _logger.LogWarning("Service run failed for {Url}! Timeout occured! Will be rescheduled",
                                    resultWrapper.Result?[i].Urls?.Self ?? "");
                                break;
                            }
                            case ServiceHelpers.ResultWrapper<Foodbank>.Code.Errored:
                            {
                                _logger.LogWarning(
                                    "Service run failed for {Url}! Error occured! Will be rescheduled {Exception}",
                                    resultWrapper.Result?[i].Urls?.Self ?? "", resultWrapper.Ex?.ToString() ?? "");
                                break;
                            }
                            case ServiceHelpers.ResultWrapper<Foodbank>.Code.Cancelled:
                            {
                                _logger.LogWarning(
                                    "Service run failed for {Url}! Cancellation occured! Will be rescheduled {Ex}",
                                    resultWrapper.Result?[i].Urls?.Self ?? "", resultWrapper.Ex?.ToString() ?? "");
                                break;
                            }
                        }
                    }

                    _logger.LogInformation("Service run complete!");

                    break;
                }
                case ServiceHelpers.ResultWrapper<List<Foodbank>>.Code.Timeout:
                {
                    _logger.LogWarning("Service run failed! Timeout occured! Will be rescheduled");
                    break;
                }
                case ServiceHelpers.ResultWrapper<List<Foodbank>>.Code.Errored:
                {
                    _logger.LogWarning("Service run failed! Error occured! Will be rescheduled {Exception}",
                        resultWrapper.Ex);
                    break;
                }
                case ServiceHelpers.ResultWrapper<List<Foodbank>>.Code.Cancelled:
                {
                    _logger.LogWarning("Service run failed! Cancellation occured! Will be rescheduled {Exception}",
                        resultWrapper.Ex);
                    break;
                }
            }

            await Task.Delay(100, stoppingToken);
        }
    }

    /// <summary>
    ///     > The Dispose() function is called when the service is stopped
    /// </summary>
    public override void Dispose()
    {
        _logger.LogInformation("Gracefully stopping service...");
        _httpClient.Dispose();
        _logger.LogInformation("Goodbye!");
        base.Dispose();
    }

    /// <summary>
    ///     This function makes an HTTP GET request to the specified URL, and returns the response as a JSON object
    /// </summary>
    /// <param name="url">The URL to the API endpoint</param>
    /// <param name="cancellationToken">This is a token that can be used to cancel the request.</param>
    /// <returns>
    ///     A Task&lt;T&gt; that contains reading asynchronous execution
    /// </returns>
    private async Task<T> GetFoodbank<T>(string url, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<T>(cancellationToken);
    }
}