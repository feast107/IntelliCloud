using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestAndroid
{
    internal class Client
    {
        private readonly static HttpClient _client = new HttpClient();
        public static HttpClient client { get { return client; } }

        public async Task<string> SendMessage()
        {
            throw new NotImplementedException();
            HttpResponseMessage responseMessage =await _client.PostAsync("",new ByteArrayContent(new byte[] { }));
            return responseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
