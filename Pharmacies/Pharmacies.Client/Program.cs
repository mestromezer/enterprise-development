using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pharmacies.Client;
using Pharmacies.Client.Helpers;
using Pharmacies.Client.OpenApiService;
using Pharmacies.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IPharmaciesApiWrapper, PharmaciesApiWrapper>();

builder.Services.AddScoped(x => {
    var apiUrl = new Uri(builder.Configuration["OpenApi:ServerUrl"]);

    // use fake backend if "fakeBackend" is "true" in appsettings.json
    if (builder.Configuration["fakeBackend"] == "true")
        return new HttpClient(new FakeBackendHandler()) { BaseAddress = apiUrl };

    return new HttpClient() { BaseAddress = apiUrl };
});

var host = builder.Build();

var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
await authenticationService.Initialize();

await host.RunAsync();
