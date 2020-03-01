using Newtonsoft.Json;
using SimpleBinaryComparer.Domain.Service.Model;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.IntegrationTest
{
    public static class ResponseExtension
    {
        public static async Task<ResponseBase> ToResponseBaseAsync(this HttpResponseMessage httpResponseMessage)
        {
            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            using (var streamReader = new StreamReader(contentStream))
            {
                var strm = streamReader.ReadToEnd();

                return JsonConvert.DeserializeObject<ResponseBase>(strm);
            }
        }
    }
}