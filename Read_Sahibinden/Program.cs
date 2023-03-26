var host = Injections.MyConfigurationService();
var myService = host.Services.GetRequiredService<IReadDataService>();
Console.WriteLine(await myService.ReadWebPageData());
Console.ReadLine();