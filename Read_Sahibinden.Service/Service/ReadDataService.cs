namespace Read_Sahibinden.Service;
public class ReadDataService : IReadDataService
{
    HttpClient _httpClient;

    public ReadDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "me");
    }

    public async Task<List<string>> ReadWebPageData()
    {
        List<string> datas = new List<string>();

        var response = await _httpClient.GetAsync("https://www.sahibinden.com");
        Stream stream= await response.Content.ReadAsStreamAsync();
        HtmlDocument document= new HtmlDocument();
        document.Load(stream);
        var iTagList = document.DocumentNode.SelectNodes("//ul[@class='vitrin-list clearfix']//li");
        foreach ( var item in iTagList) {
            var item2 = item;
            var doc = new HtmlDocument();
            doc.LoadHtml(item2.InnerHtml);
            var a = doc.DocumentNode.Descendants("a").First();
            string hrefValue = a.Attributes["href"].Value;
        }
        return datas;
    }
}
