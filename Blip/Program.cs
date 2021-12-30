using Blip;
using Blip.Services;
using Chip;
using Chip.Output;
using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IHowl, Howl>();
builder.Services.AddSingleton<ISound, SoundService>();
builder.Services.AddSingleton<Emulator>();
builder.Services.AddSingleton<KeyMappingsService>();

await builder.Build().RunAsync();
