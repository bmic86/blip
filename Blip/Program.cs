using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blip;
using Blip.Services;
using Chip;
using Chip.Output;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddBlazorise()
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddSingleton<ISound, SoundService>();
builder.Services.AddSingleton<EmulatorService>();
builder.Services.AddSingleton<Emulator>();
builder.Services.AddSingleton<KeyMappingsService>();

builder.Services.AddScoped<GamesLibraryService>();

await builder.Build().RunAsync();
