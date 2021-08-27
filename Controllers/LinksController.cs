using Croncher.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Croncher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly ILinksService _linksService;

        public LinksController(ILinksService linksService)
        {
            _linksService = linksService;
        }

        public async Task<ActionResult> PostLink()
        {
            var link = await getLinkstringFromRequest(Request);

            var encodedId = await _linksService.InsertLinkAsync(link);

            return Ok(JsonConvert.SerializeObject(new { encodedId = encodedId }));
        }

        private async Task<string> getLinkstringFromRequest(HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var definition = new { url = "" };

                var json = await reader.ReadToEndAsync();
                var jsonObj = JsonConvert.DeserializeAnonymousType(json, definition);

                return jsonObj.url;
            }
        }
    }
}
