using Croncher.Helpers;
using Croncher.Models;
using System.Threading.Tasks;

namespace Croncher.Services
{
    public class LinksService : ILinksService
    {
        private readonly LinkContext _context;

        public LinksService(LinkContext context)
        {
            _context = context;
        }

        public async Task<string> GetLinkAsync(string encodedId)
        {
            int id = IntBaseConverter.Base62ToBase10(encodedId);

            var link = await _context.Links.FindAsync(id);

            return link?.Url;
        }

        public async Task<string> InsertLinkAsync(string url)
        {
            url = "http://" + url;
            Link link = new Link() { Url = url };

            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            var convertedId = IntBaseConverter.Base10ToBase62(link.Id);

            return convertedId;
        }
    }
}
