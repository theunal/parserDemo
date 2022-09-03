using Data.Models;

namespace Data.Parser
{
    public interface IZParser
    {
        Task<List<ZModel>> GetData(string value);
    }
}
