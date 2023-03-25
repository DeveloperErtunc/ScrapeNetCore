var host = Injections.MyConfigurationService();
var myService = host.Services.GetRequiredService<IReadDataService>();
var webPage = await myService.ReadWebPageData();
Console.WriteLine(webPage);
Console.ReadLine();