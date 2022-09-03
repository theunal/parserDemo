using RestSharp;

namespace Data.Data
{
    public class DataRepository : IDataRepository
    {
        public async Task<string> ZafiyetPost(string value)
        {
            var client = new RestClient("https://zafiyet.org/getresults");
            var request = new RestRequest();
            request.Method = Method.Post;

            // header
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("user-agent", 
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

            // body
            request.AddParameter("searchkey", value);
            request.AddParameter("year", "9999");
            request.AddParameter("field", "1");

            request.AlwaysMultipartFormData = true;

            var response = await client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
