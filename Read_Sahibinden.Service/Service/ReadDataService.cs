using System.Net;

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

    public async Task ReadWebPageData()
    {
        List<AdvertisementMinModel> datas = new List<AdvertisementMinModel>();

        var response = await _httpClient.GetAsync("");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            Stream stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument document = new HtmlDocument();
            document.Load(stream);
            var iTagList = document.DocumentNode.SelectNodes("//ul[@class='vitrin-list clearfix']//li");
            foreach (var item in iTagList)
            {
                var href = GetHref(item);
                var data = await GetAdvertisementMinModel(href);
                if (data != null)
                    datas.Add(data);
            }
            var dataStr = JsonSerializer.Serialize(datas);
            if (!string.IsNullOrEmpty(dataStr))
                WriteData(dataStr);

            Console.WriteLine(dataStr);
            Console.WriteLine("Ortalama fiyat = " + datas.Average(x => x.DecimalPrice));

        }
        else Console.WriteLine("Çalışmadı");
        
    }
    private string GetHref(HtmlNode htmlNode)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlNode.InnerHtml);
        var a = doc.DocumentNode.SelectSingleNode("a");
        return a.Attributes["href"].Value;
    }
    private async Task<AdvertisementMinModel?> GetAdvertisementMinModel(string hrefDetail)
    {
        var newResponse = await _httpClient.GetAsync(hrefDetail);
        Stream newStream = await newResponse.Content.ReadAsStreamAsync();
        HtmlDocument doc = new HtmlDocument();
        doc.Load(newStream);
        if (doc.Text.Contains("classifiedInfo") && doc.Text.Contains("classifiedDetailTitle"))
        {
            var detailPrice = doc.DocumentNode.SelectSingleNode("//div[@class='classifiedInfo ']//h3");
            var detailTitle = doc.DocumentNode.SelectSingleNode("//div[@class='classifiedDetailTitle']//h1").InnerHtml ?? string.Empty;
            if (detailPrice != null && !string.IsNullOrEmpty(detailTitle))
            {
                var clean = detailPrice.InnerText.Replace(" ", "").Replace("/", "").Replace("\n", "");
                var index = clean.IndexOf("TL");
                return  new AdvertisementMinModel
                {
                    Title = detailTitle,
                    Price =   clean.Substring(0, index + 2),
                    DecimalPrice = Convert.ToDecimal(clean.Substring(0, index ))
                };
            }
        }
        return null;
    }
    private void WriteData(string dataStr)
    {
        DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
        string directory = info.Parent.Parent.Parent.FullName + $"\\MyLog\\{Guid.NewGuid()}.txt";
        FileStream fs = new FileStream(directory, FileMode.OpenOrCreate, FileAccess.Write);
        fs.Close();
        File.AppendAllText(directory, Environment.NewLine + dataStr);
    }
}
