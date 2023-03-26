var host = Injections.MyConfigurationService();
var myService = host.Services.GetRequiredService<IReadDataService>();
var liItems = JsonConvert.SerializeObject(await myService.ReadWebPageData());
Console.WriteLine(liItems);
Console.ReadLine();