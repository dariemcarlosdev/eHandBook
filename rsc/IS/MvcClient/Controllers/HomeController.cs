using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models;
using MvcClient.Services;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        //Here we inject token service.
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService _tokenService;

        public HomeController(ILogger<HomeController> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Weather()
        {
            //list to hold weather data we will get back from api.
            var data = new List<WeatherData>();

            //when I create the client, i get token reponse form Token Service, and then on my http client use
            //ext method .SetBearerToken to add this token to the request.
            using (var client = new HttpClient())
            {
                var tokenReponse = await _tokenService.GetTokenAsync("weatherapi.read"); //here defining scope:"api1.read", I'm saying I want a token that allow me to read from this api.

                client
                    .SetBearerToken(tokenReponse.AccessToken);

                var result = client
                    .GetAsync("https://localhost:6001/WeatherForecast")
                    .Result;

                if (result.IsSuccessStatusCode)
                {
                    var model = result.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<List<WeatherData>>(model);
                    
                    return View(data);
                }

                else
                {
                    throw new Exception("Unable to get content");
                }
            }
        
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}