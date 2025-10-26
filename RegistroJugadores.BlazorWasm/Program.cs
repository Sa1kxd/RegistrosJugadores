using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RegistroJugadores.BlazorWasm;
using RegistroJugadores.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://gestionhuacalesapi.azurewebsites.net/") });

builder.Services.AddScoped<IPartidasApiService, PartidasAPIService>();
builder.Services.AddScoped<IMovimientoAPIService, MovimientoAPIService>();

await builder.Build().RunAsync();
