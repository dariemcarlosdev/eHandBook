using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BlazorClient.Shared
{
    public partial class RedirectToLogin
    {
        [Inject] public NavigationManager Navigation {get; set;}
        [Inject] private ILogger<RedirectToLogin> Logger { get; set; } = default!;
        public string BlazorRocksText { get; set; } =
        "Blazor rocks the browser!";

        protected override void OnInitialized()
        {

            Logger.LogInformation("Initialization code of RedirectToLogin executed!");
            Navigation.NavigateTo($"/login?redirectUri={Uri.EscapeDataString(Navigation.Uri)}", true);
        }
    }
}
