using System;
using System.Collections.Generic;
using System.Text;

namespace IntelliHttpClient
{
 
    public enum Routes
    {
        api,
        Access,
        File,
        Picture,
    }

    public enum Files
    {
        Excel,
        Example,
        Word,
        Run,
        Pause,
        ExcelTasks,
        WordTasks,
        WordControl,
        ExcelControl
    }

    public enum Access
    {
        Login,
        Logout,
        Regist,
        Current,
        User
    }
    public enum Picture
    {
        Submit,
        Upload,
        Ask,
        Wait
    }

    public static class Route
    {
        public const string Address = "192.168.101.108";
        public const string Port = "5000";
        public static string GetAddress()
        {
            return "http://" + Address + ":" + Port ;
        }
         
    }
}
