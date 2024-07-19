using BlazorClient.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BlazorClient.Pages
{
    public partial class Weather
    {
        private List<WeatherForecastModel> WeatherForecasts = new();
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IConfiguration Config { get; set; }
        //Injecting token Service to add BT to http request.
        [Inject] private ITokenService TokenService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var tokenResponse = await TokenService.GetTokenAsync("weatherapi.read");
            //binding bearer token to the actual http request header.
            HttpClient.SetBearerToken(tokenResponse.AccessToken);
            
            var result = await HttpClient
                .GetAsync(Config["apiUrl"] + "/WeatherForecast");
                

            if (result.IsSuccessStatusCode)
            {
                WeatherForecasts = await result.Content.ReadFromJsonAsync<List<WeatherForecastModel>>();
            }

            else
            {
                throw new Exception("Unable to get content");
            }
        }
    }
}
