using Data.Models;
using Data.Parser;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IZParser zParser;

        public DataController(IZParser zParser)
        {
            this.zParser = zParser;
        }

        [HttpGet("getData")]
        public async Task<List<ZModel>> GetData(string value)
        {
            return await zParser.GetData(value);
        }
    }
}
