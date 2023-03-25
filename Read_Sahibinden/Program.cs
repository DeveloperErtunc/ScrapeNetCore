var host = Injections.MyConfigurationService();
var myService = host.Services.GetRequiredService<IReadDataService>();
var liItems = await myService.ReadWebPageData();
Console.WriteLine(string.Join(",",liItems));
Console.ReadLine();