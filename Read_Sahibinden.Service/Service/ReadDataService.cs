namespace Read_Sahibinden.Service;
public class ReadDataService : IReadDataService
{
    HttpClient _httpClient;
    public ReadDataService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }
    public async Task<string> ReadWebPageData()
    {
        var response = await _httpClient.GetAsync("https://www.sahibinden.com");
        Stream stream= await response.Content.ReadAsStreamAsync();
        HtmlDocument document= new HtmlDocument();
        document.Load(stream);
        var dataOfHtml = document.DocumentNode.SelectNodes("//ul[@class='vitrin-list clearfix']");
        foreach (HtmlNode data in dataOfHtml)
        {
           var dataStr = data.InnerHtml;
        }
        return document.ToString() ?? string.Empty;
    }
}
