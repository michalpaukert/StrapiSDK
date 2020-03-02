using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strapi_SDK
{
    public interface IStrapiClient
    {
        Task<int> Count(string contentType);
        void Dispose();
        Task<IEnumerable<T>> GetEntries<T>(string contentType);
        Task<T> GetEntry<T>(string contentType, int id);
        Task<byte[]> GetFile(string id);
        Task Login(string username, string password);
    }
}