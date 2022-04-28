using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace IntelliHttpClient
{
    public interface IHttp
    {
        string Get(string url);
        string Get(string url, string arg, string value);
        string Get(string url,KeyValuePair<string,string> param);
        string Get(string url, Dictionary<string, string> param);

        Stream Download(string url);
        Task<string> GetAsync(string url);
        Task<string> GetAsync(string url, string arg, string value);
        Task<string> GetAsync(string url, KeyValuePair<string, string> param);
        Task<string> GetAsync(string url, Dictionary<string, string> param);


        string Post(string url, HttpContent content);

        string Post(string url, string JSON);
        Task<string> PostAsync(string url, HttpContent content);

        Task<string> PostAsync(string url, string JSON);
    }
}
