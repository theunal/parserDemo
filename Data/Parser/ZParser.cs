using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Data.Data;
using Data.Models;

namespace Data.Parser
{
    public class ZParser : IZParser
    {
        private readonly IDataRepository dataRepository;

        public ZParser(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public async Task<List<ZModel>> GetData(string value)
        {
            var response = await dataRepository.ZafiyetPost(value);

            var htmlParser = new HtmlParser();
            var parsedData = htmlParser.ParseDocument(response);
            var row = parsedData.All.Where(x => x.GetAttribute("class") == "row").ToList();

            var list = new List<ZModel>();

            foreach (var item in row)
            {
                // cve
                var CveDiv = item.GetElementsByClassName("card-header bg-success text-light").FirstOrDefault();
                var cveB = CveDiv?.GetElementsByTagName("b").ToList().LastOrDefault();
                var cve = cveB is not null ? cveB.Text().Trim() : "";

                // vuln type
                var VulnDiv = item.GetElementsByClassName("col-md-12 text-center border").FirstOrDefault();
                var vulnTypeB = VulnDiv?.GetElementsByTagName("b").FirstOrDefault();
                var vulnType = vulnTypeB is not null ? vulnTypeB?.Text().Replace("Bulgu Tipi", "") : "";

                // dates
                var dateDivList = item.GetElementsByClassName("col-md-6 text-center border").ToList();
                var publishedDateB = dateDivList.FirstOrDefault()?.GetElementsByTagName("b").FirstOrDefault();
                var modifiedDateB = dateDivList.LastOrDefault()?.GetElementsByTagName("b").FirstOrDefault();
                var publishedDate = publishedDateB is not null ?
                    publishedDateB?.Text().Replace("Yayın Tarihi", "").Trim() : "";
                var modifiedDate = modifiedDateB is not null ?
                    modifiedDateB?.Text().Replace("Değiştirilme Tarihi", "").Trim() : "";

                // risk score
                var riskScoreDiv = item.GetElementsByClassName("col-md-12 border text-center").LastOrDefault();
                var riskScoreB = riskScoreDiv?.GetElementsByTagName("b").FirstOrDefault();
                var riskScore = riskScoreB is not null ?
                    riskScoreB?.Text().Replace("Risk Skoru", "").Trim() : "";

                // description
                var descDiv = item.GetElementsByClassName("row p-2").FirstOrDefault();
                var descB = descDiv?.GetElementsByTagName("b").FirstOrDefault();
                var desc = descB is not null ? descB?.Text().Trim() : "";

                list.Add(CreateZModel(cve, vulnType, publishedDate, modifiedDate, riskScore, desc));
            }

            return list;
        }

        private static ZModel CreateZModel(string cve, string? vulnType, string? publishedDate, string? modifiedDate, string? riskScore, string? desc)
        {
            return new ZModel
            {
                Cve = cve,
                VulnType = vulnType,
                PublicationDate = publishedDate,
                ModificationDate = modifiedDate,
                RiskScore = riskScore,
                Description = desc
            };
        }
    }
}
