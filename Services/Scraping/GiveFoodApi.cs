#region

using System.Diagnostics;
using System.Net.Http.Headers;

#endregion

namespace Foodbank_Project.Jobs.Scraping;

// ReSharper disable once ClassNeverInstantiated.Global
/**public class GiveFoodApiFoodBanks : IJob
{
    private readonly ILogger<GiveFoodApiFoodBanks> _logger;

    public GiveFoodApiFoodBanks(ILogger<GiveFoodApiFoodBanks> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Commencing GiveFood API Scraping job...");
        var stopwatch = new Stopwatch();
        List<Foodbank> foodbanks;
        // Stage 1
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.givefood.org.uk/api/2/foodbanks/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            try
            {
                stopwatch.Start();
                var response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    stopwatch.Stop();
                    foodbanks = await response.Content.ReadAsAsync<List<Foodbank>>();
                    _logger.LogInformation("Finished gathering foodbank centres. Got {Count} in {Elapsed}ms",
                        foodbanks.Count, stopwatch.ElapsedMilliseconds);
                    response.Dispose();
                    client.Dispose();
                }
                else
                {
                    response.Dispose();
                    client.Dispose();
                    throw new Exception(
                        $"HttpClient responded with non 200 code: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                client.Dispose();
                _logger.LogError("Something went really wrong! Job will be skipped! {Exception}", ex);
                return;
            }
        }
        // Stage 2
        {
            var client = new HttpClient();
            for (var i = 0; i < foodbanks.Count; i++)
            {
                var foodbank = foodbanks[i];
                if (foodbank.Urls?.Self is null) continue;
                var request = new HttpRequestMessage(HttpMethod.Get, foodbank.Urls.Self);
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

                try
                {
                    stopwatch.Restart();
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        foodbanks[i].Merge(await response.Content.ReadAsAsync<Foodbank>());
                        stopwatch.Stop();
                        _logger.LogInformation(
                            "Successfully added locations for {Name}({Self}) in {Elapsed}ms [{I} of {Count}]",
                            foodbank.Name, foodbank.Urls.Self, stopwatch.ElapsedMilliseconds, i + 1, foodbanks.Count);
                        Thread.Sleep(100);
                    }
                    else
                    {
                        response.Dispose();
                        request.Dispose();
                        throw new Exception(
                            $"HttpClient responded with non 200 code: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    request.Dispose();
                    _logger.LogError("Something went really wrong! Skipping... {Exception}", ex);
                }
            }

            client.Dispose();
        }
        _logger.LogInformation("GiveFood API Scraping job finished");

        // ReSharper disable once UnusedVariable
        var internalFoodbanks = foodbanks.Select(FoodbankConverter.Convert).ToList();
        internalFoodbanks.ForEach(f => f.Provider = Provider.GiveFoodApi);
        // TODO Submit data to DB
    }
}
**/


public class GiveFoodApiService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    private readonly HttpClient _httpClient = new();
    private readonly Stopwatch _sw = new();
    
    public GiveFoodApiService(ILoggerFactory logger, IConfiguration configuration)
    {
        _logger = logger.CreateLogger("Services.GiveFoodApi");
        _config = configuration.GetSection("Services:GiveFoodApi");
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        /*
        try
        {
            _logger.LogInformation("Service started. Runs every {Stamp}", TimeSpan.FromSeconds(_config.GetValue<int>("Interval")).ToString(@"h\hm\ms\s"));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_config.GetValue<int>("Interval") * 1000, stoppingToken);
                    _logger.LogInformation("Service running");

                    var cancelTask = Task.Delay(_config.GetValue<int>("Timeout") * 1000, stoppingToken);
                    var giveFoodApiTask =
                        Task.Run(() => GetFoodbank<List<Foodbank>>("https://www.givefood.org.uk/api/2/foodbanks/"), stoppingToken);

                    await await Task.WhenAny(giveFoodApiTask, cancelTask);

                    if (giveFoodApiTask.IsCompletedSuccessfully)
                    {
                        _logger.LogInformation("Successfully got {Count} foodbanks", giveFoodApiTask.Result.Count);
                        foreach (var foodbankRaw in giveFoodApiTask.Result)
                        {
                            cancelTask = Task.Delay(_config.GetValue<int>("Timeout") * 1000, stoppingToken);
                            var foodbank = Task.Run(() => GetFoodbank<Foodbank>(foodbankRaw.Urls?.Self ?? ""));

                            await await Task.WhenAny(foodbank, cancelTask);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Service run failed! Timeout occured! Will be rescheduled");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Service run failed! Will be rescheduled {Exception}", e);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Service failed to start! Will not be scheduled again until restart {Exception}", e);
        }*/
        
        _logger.LogInformation("Service started. Runs every {Stamp}", TimeSpan.FromSeconds(_config.GetValue<int>("Interval")).ToString(@"h\hm\ms\s"));
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(100);
        }
    }
    
    
    public override void Dispose()
    {
        _logger.LogInformation("Gracefully stopping service...");
        _httpClient.Dispose();
        _logger.LogInformation("Goodbye!");
        base.Dispose();
    }

    // Url : Country
    private async Task<T> GetFoodbank<T>(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );
        _sw.Restart();
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<T>();
        }
        throw new Exception($"Non 200 HTTP code for {url}");
    }
}