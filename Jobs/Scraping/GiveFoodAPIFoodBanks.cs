using Foodbank_Project.Models;
using Quartz;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Foodbank_Project.Jobs.Scraping
{
    public class GiveFoodAPIFoodBanks : IJob
    {
        private readonly ILogger<GiveFoodAPIFoodBanks> _logger;

        public GiveFoodAPIFoodBanks(ILogger<GiveFoodAPIFoodBanks> logger)
        {
            _logger = logger;
        }

        // TODO: Cleanup
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Give Food API Scrapper Job Started");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://www.givefood.org.uk/api/2/foodbanks/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );


                var sw = new Stopwatch();
                HttpResponseMessage response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    List<Foodbank> foodbanks = await response.Content.ReadAsAsync<List<Foodbank>>();
                    sw.Stop();
                    _logger.LogInformation($"Successfully gathered {foodbanks.Count} foodbanks in {sw.ElapsedMilliseconds}ms");

                    HttpClient subClient = new HttpClient();
                    for (int i = 0; i < foodbanks.Count; i++)
                    {
                        var _foodbank = foodbanks[i];
                        if (_foodbank.Urls is not null && _foodbank.Urls.Self is not null)
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, _foodbank.Urls.Self);
                            request.Headers.Accept.Clear();
                            request.Headers.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json")
                            );
                            sw.Restart();
                            HttpResponseMessage subResponse;
                            try
                            {
                                subResponse = await subClient.SendAsync(request);
                            }catch (Exception ex)
                            {
                                _logger.LogError($"Something went very wrong! {ex.ToString()}. Skipping...");
                                continue;
                            }
                            if (subResponse.IsSuccessStatusCode)
                            {
                                foodbanks[i].Merge(await subResponse.Content.ReadAsAsync<Foodbank>());
                                sw.Stop();
                                _logger.LogInformation($"Succesfully added locations for {_foodbank.Name}({_foodbank.Urls.Self}) in {sw.ElapsedMilliseconds}ms [{i+1} of {foodbanks.Count}]");
                                Thread.Sleep(200);
                            }
                            else
                            {
                                _logger.LogError($"HTTP ERROR: {(int)subResponse.StatusCode} ({subResponse.ReasonPhrase}). Skipping...");
                            }
                        }
                        else
                        {
                            _logger.LogError($"Something went really wrong! Skipping {foodbanks[i].Name ?? "NAME MISSING"}. Skipping...");
                            continue;

                        }

                    }
                    subClient.Dispose();
                    {
                        var count = 0;
                        foreach (var item in foodbanks)
                        {
                            count += item.Locations?.Count ?? 0;
                        }
                        _logger.LogInformation($"Successfully gathered {count} foodbank locations");
                    }
                    // TODO: Stuff with foodbanks
                }
                else
                {
                    _logger.LogError($"HTTP ERROR: {(int)response.StatusCode} ({response.ReasonPhrase}). Job aborted");
                }

                client.Dispose();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went very wrong! {ex.ToString()}. Job aborted");
            }
        }
    }

}
