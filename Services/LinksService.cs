using Croncher.Helpers;
using Croncher.Models;
using System;
using System.Net;
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
            var saneUrl = UrlSanityCheck(url);

            Link link = new Link() { Url = saneUrl };
            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            var convertedId = IntBaseConverter.Base10ToBase62(link.Id);

            return convertedId;
        }

        private string UrlSanityCheck(string url)
        {
            if (ValidateUrl(url)) return url;
            else return TryMakeUrlValid(url);
        }

        private bool ValidateUrl(string url)
        {
            //Is it well formed?
            Uri fullyQualifiedUri;
            var wellFormed = Uri.TryCreate(url, UriKind.Absolute, out fullyQualifiedUri)
                && (fullyQualifiedUri.Scheme == Uri.UriSchemeHttp || fullyQualifiedUri.Scheme == Uri.UriSchemeHttps)
                && Uri.IsWellFormedUriString(url, UriKind.Absolute);

            if (!wellFormed) return false;

            //Is it reachable?
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 5000;
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch(WebException)
            {
                return false;
            }
        }

        private string TryMakeUrlValid(string url)
        {
            var prefixed = "http://" + url;
            
            //If it's *still* bad...
            if(!ValidateUrl(prefixed))
            {
                throw new ArgumentException("URL does not appear to be valid.");
            }

            return prefixed;
        }
    }
}
