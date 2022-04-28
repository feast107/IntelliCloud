using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntelliTool
{
    public  class AppMessage 
    {
        public AppMessage()
        {

        }

        public static string GetStandaredMessage(string message,int length)
        {
            if (message.Length >= length)
            {
                return message;
            }
            int c =length - message.Length;
            if (c % 2 != 0)
            {
                message += ' ';
                c--;
            }
            c /= 2;
            for(int i = 0; i < c; i++)
            {
                message = ' ' + message + ' ';
            }
            return message;
        }

        public static string GetStanderedMessage(List<string> messages)
        {
            int length = 0;
            messages.ForEach(x => 
            {
                if (x.Length > length)
                {
                    length = x.Length;
                }
            });
            string ret = string.Empty;
            messages.ForEach(x => { ret += GetStandaredMessage(x, length) + '\n'; });
            return ret;
        }
    }
}
