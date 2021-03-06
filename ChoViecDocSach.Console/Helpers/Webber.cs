using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Onha.Kiet
{
   public class Webber
    {
        HttpClient _client;
        public Webber(string hostname)
        {
            // at company
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = false,
                DefaultProxyCredentials = CredentialCache.DefaultCredentials,
                Credentials = CredentialCache.DefaultCredentials
            };

            _client = new HttpClient(handler)
            {
                MaxResponseContentBufferSize = 1000000
            };

            if (!string.IsNullOrEmpty(hostname))
            {
                _client.BaseAddress = new Uri(hostname);
            }

        }

        public Task<string> GetStringAsync(string path)
        {
            return _client.GetStringAsync(path);
        }

        public Task<byte[]> DownloadFile(string path)
        {
            Task<byte[]> buffer =  _client.GetByteArrayAsync(path); 
            
            return buffer;
        }
    }
}