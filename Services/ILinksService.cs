using System.Threading.Tasks;

namespace Croncher.Services
{
    public interface ILinksService
    {
        public Task<string> GetLinkAsync(string encodedId);

        public Task<string> InsertLinkAsync(string url);
    }
}
