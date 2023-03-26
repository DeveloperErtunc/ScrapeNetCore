namespace Read_Sahibinden.Service;
public class ReadDataService : IReadDataService
{
    HttpClient _httpClient;

    public ReadDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "me");
        _httpClient.BaseAddress =new Uri("https://www.sahibinden.com");
    }

    public async Task<List<string>> ReadWebPageData()
    {
        List<string> datas = new List<string>();

        var response = await _httpClient.GetAsync("");
        Stream stream= await response.Content.ReadAsStreamAsync();
        HtmlDocument document= new HtmlDocument();
        document.Load(stream);
        var iTagList = document.DocumentNode.SelectNodes("//ul[@class='vitrin-list clearfix']//li");
        foreach ( var item in iTagList) {
            var item2 = item;
            var doc = new HtmlDocument();
            doc.LoadHtml(item2.InnerHtml);
            var a = doc.DocumentNode.SelectSingleNode("a");
            string hrefValue = a.Attributes["href"].Value;
            var newResponse = await _httpClient.GetAsync(hrefValue);
            Stream newStream = await newResponse.Content.ReadAsStreamAsync();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(newStream);
            if(htmlDocument.Text.Contains("classifiedInfo"))
            {
                string price = string.Empty;
                var ilanDetay = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='classifiedInfo ']");
                if(ilanDetay != null)
                    price = ilanDetay.InnerText;
                    
            }
        }
        return datas;
    }
}
