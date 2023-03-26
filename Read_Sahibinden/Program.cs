var host = Injections.MyConfigurationService();
var myService = host.Services.GetRequiredService<IReadDataService>();
await myService.ReadWebPageData();
Console.ReadLine();