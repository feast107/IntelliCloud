using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// 通过HttpListener实现
/// </summary>
public class WebServer
{
    private HttpListener listener = new HttpListener();
    private Dictionary<string, Action> actionDict = new Dictionary<string, Action>();

    public void AddPrefixes(string url, Action action)
    {
        actionDict.Add(url, action);
    }
    public void Start(int port)
    {
        if (actionDict.Count <= 0)
        {
            System.Console.WriteLine("没有在监听的接口");
            return;
        }
        foreach (var item in actionDict)
        {
            var url = string.Format("http://127.0.0.1:{0}{1}", port, item.Key + "/");
            System.Console.WriteLine(url);
            listener.Prefixes.Add(url);
        }
        listener.Start();
        listener.BeginGetContext(Result, null);
        System.Console.WriteLine("开始监听");
        System.Console.ReadKey();
    }

    private void Result(IAsyncResult ar)
    {
        listener.BeginGetContext(Result, null);
        var context = listener.EndGetContext(ar);
        var req = context.Request;
        var rsp = context.Response;

        rsp.ContentType = "text/plain;charset=UTF-8";//告诉客户端返回的ContentType类型为纯文本格式，编码为UTF-8
        rsp.AddHeader("Content-type", "text/plain");//添加响应头信息
        rsp.ContentEncoding = Encoding.UTF8;

        HandlerReq(req.RawUrl);
        try
        {
            using (var stream = rsp.OutputStream)
            {
                byte[] data = Encoding.Default.GetBytes("Hello World");
                stream.Write(data, 0, data.Length);
            }
        }
        catch (System.Exception e)
        {
            rsp.Close();
            System.Console.WriteLine(e);
        }
        rsp.Close();
    }
    /// <summary>
    /// 处理请求
    /// </summary>
    private void HandlerReq(string url)
    {
        try
        {
            var action = actionDict[url];
            action();
        }
        catch (System.Exception e)
        {
            System.Console.WriteLine(e);
        }
    }
}
