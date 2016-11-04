using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YCApp.RPCDemo
{
    public class Common
    {
        public static Dictionary<string,string> GetArgs(string[] args)
        {
            Dictionary<string, string> dic = null;
            if (args != null)
            {
                dic= new Dictionary<string, string>(); 
                foreach (string item in args)
                {
                    if(!string.IsNullOrWhiteSpace(item) && item.Contains("="))
                    {
                        var keys = item.Split('=');
                        dic[keys[0]] = keys[1];
                    }
                }
            }
            return dic;
        }
    }
}
