using Foodbank_Project.Models;
using Quartz;
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

        public async Task Execute(IJobExecutionContext context)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.givefood.org.uk/api/2/foodbanks/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                List<Foodbank> foodbanks = await response.Content.ReadAsAsync<List<Foodbank>>();
                
                // TODO: Something with this data
               
                
            }
            else
            {
                _logger.LogError($"HTTP ERROR: {(int)response.StatusCode} ({response.ReasonPhrase})");
            }

            client.Dispose();

        }
    }
    
}
