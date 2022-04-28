using Foodbank_Project.Models.Foodbank;
using Foodbank_Project.Models.Foodbank.External;
using Foodbank_Project.Models.Foodbank.Internal;
using Quartz;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Foodbank_Project.Jobs.Scraping
{
    public class GiveFoodAPIFoodBanks : IJob
    {
        private readonly ILogger<GiveFoodAPIFoodBanks> _logger;

        public GiveFoodAPIFoodBanks(ILogger<GiveFoodAPIFoodBanks> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Commencing GiveFood API Scraping job...");
            Stopwatch stopwatch = new Stopwatch();
            List<Models.Foodbank.External.Foodbank> foodbanks;
            // Stage 1
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://www.givefood.org.uk/api/2/foodbanks/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

                try
                {
                    stopwatch.Start();
                    HttpResponseMessage response = await client.GetAsync("");
                    if (response.IsSuccessStatusCode)
                    {
                        stopwatch.Stop();
                        foodbanks = await response.Content.ReadAsAsync<List<Models.Foodbank.External.Foodbank>>();
                        _logger.LogInformation($"Finished gathering foodbank centres. Got {foodbanks.Count} in {stopwatch.ElapsedMilliseconds}ms");
                        response.Dispose();
                        client.Dispose();
                    }
                    else
                    {
                        response.Dispose();
                        client.Dispose();
                        throw new Exception($"HttpClient responded with non 200 code: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }catch (Exception ex)
                {
                    client.Dispose();
                    _logger.LogError($"Something went really wrong! Job will be skipped! {ex}");
                    return;
                }
            }
            // Stage 2
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request;
                Models.Foodbank.External.Foodbank foodbank;
                for (int i = 0; i < foodbanks.Count; i++)
                {
                    foodbank = foodbanks[i];
                    if (foodbank.Urls is not null && foodbank.Urls.Self is not null)
                    {
                        request = new HttpRequestMessage(HttpMethod.Get, foodbank.Urls.Self);
                        request.Headers.Accept.Clear();
                        request.Headers.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json")
                        );

                        try
                        {
                            stopwatch.Restart();
                            HttpResponseMessage response = await client.SendAsync(request);
                            if (response.IsSuccessStatusCode)
                            {
                                foodbanks[i].Merge(await response.Content.ReadAsAsync<Models.Foodbank.External.Foodbank>());
                                stopwatch.Stop();
                                _logger.LogInformation($"Succesfully added locations for {foodbank.Name}({foodbank.Urls.Self}) in {stopwatch.ElapsedMilliseconds}ms [{i + 1} of {foodbanks.Count}]");
                                Thread.Sleep(100);
                            }
                            else
                            {
                                response.Dispose();
                                request.Dispose();
                                throw new Exception($"HttpClient responded with non 200 code: {response.StatusCode} - {response.ReasonPhrase}");
                            }
                        }
                        catch (Exception ex)
                        {
                            request.Dispose();
                            _logger.LogError($"Something went really wrong! Skipping... {ex}");
                            continue;
                        }

                    }
                }
                client.Dispose();
            }
            _logger.LogInformation("GiveFood API Scraping job finished");

            var internalFoodbanks = new List<Models.Foodbank.Internal.Foodbank>();
            foreach (var item in foodbanks)
            {
                internalFoodbanks.Add(FoodbankConverter.Converter(item));
            }
            // TODO Submit data to DB
        }
    }

}
