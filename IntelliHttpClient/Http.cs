using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntelliHttpClient
{
    public class Http :IHttp
    {
        private readonly HttpClient client;

        public Http()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            handler.UseCookies = true;
            handler.CookieContainer = new CookieContainer();
            handler.SslProtocols = System.Security.Authentication.SslProtocols.None;
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            client = new HttpClient(handler)
            {
                MaxResponseContentBufferSize = 2048000,
            };
        }

        private static string Attach(string url, string arg, string value)
        {
            return url +'?'+ arg + '=' + value;
        }
        private static string Attach(string url, Dictionary<string,string> param)
        {
            var i = param.GetEnumerator();
            string ret = url + '?' + i.Current.Key + '=' + i.Current.Value;
            while (i.MoveNext())
            {
                url+='&'+i.Current.Key + '=' + i.Current.Value;
            }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          return ret;                                                                    
        }

        private static Uri GetUri(string url)
        {
            return new Uri(url);
        }

        public Stream Download(string url)
        {
            var uri = GetUri(url);
            var response = client.GetAsync(uri);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                return response.Result.Content.ReadAsStreamAsync().Result;
            }
            else
            {
                throw new Exception(response.Result.StatusCode.ToString());
            }
        }

        public string Get(string url)
        {
            var uri = GetUri(url);
            var response = client.GetAsync(uri);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                return response.Result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return string.Empty;
            }
        }

        public string Get(string url, string arg, string value)
        {
            string targ = Attach(url, arg, value);
            return Get(targ);
        }

        public string Get(string url, KeyValuePair<string, string> param)
        {
            throw new NotImplementedException();
        }

        public string Get(string url, Dictionary<string, string> param)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAsync(string url)
        {
            return Task.Run(() =>Get(url));
        }

        public Task<string> GetAsync(string url, string arg, string value)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAsync(string url, KeyValuePair<string, string> param)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAsync(string url, Dictionary<string, string> param)
        {
            throw new NotImplementedException();
        }


        public string Post(string url, HttpContent content)
        {
            var response =  client.PostAsync(url, content);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                return response.Result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new Exception(response.Result.StatusCode.ToString());
            }
        }

        public string Post(string url, string JSON)
        {
            return Post(url,new StringContent(JSON, Encoding.UTF8, "application/json"));
        }

        public Task<string> PostAsync(string url, HttpContent content)
        {
            return Task.Run(() => Post(url, content));
        }

        public Task<string> PostAsync(string url, MultipartContent content)
        {
            return Task.Run(() => Post(url, content));
        }

        public Task<string> PostAsync(string url, string content)
        {
            return Task.Run(() => Post(url, content));
        }
    }
}
