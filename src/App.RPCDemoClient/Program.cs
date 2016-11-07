using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App.RPC.Model;
using Grpc.Core;
using App.RPC.Service;

using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using App.RPCDemo;

namespace YCApp.RPCDemoClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string host = "192.168.4.37";
            string port = "9007";
            string type = "1";
            long length = 10;
            var dic = Common.GetArgs(args);
            if (dic != null && dic.Count > 0)
            {
                string tempHost;
                string tempPort, tempLength;

                if (dic.TryGetValue("host", out tempHost))
                {
                    host = tempHost;
                }
                if (dic.TryGetValue("port", out tempPort))
                {
                    port = tempPort;
                }

                if (dic.TryGetValue("repeat", out tempLength))
                {
                    length = Convert.ToInt64(tempLength);
                }

                if (dic.ContainsKey("type"))
                {
                    type = dic["type"];
                }
            }

            string address = string.Format("{0}:{1}", host, port);
            switch (type)
            {
                case "2":
                    Test.TestInstance(address, length);
                    break;
                case "3":
                    Test.TestSingleton(address, length);
                    break;
                default:
                    Execute(host, port, length);
                    break;
            }

            Console.WriteLine("continue .....");
            Console.ReadLine();
        }

        public static void Execute(string host, string port, long length)
        {
            Channel channel = new Channel(string.Format("{0}:{1}", host, port), ChannelCredentials.Insecure);
            var client = new RPCDemoService.RPCDemoServiceClient(channel);

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < length; i++)
            {
                var reply = client.GetById(new DemoId() { Id = i });
                Console.WriteLine("receive" + JsonConvert.SerializeObject(reply));
            }
            stopwatch.Stop();

            Console.WriteLine(string.Format("call GetById repeat={0}, time={1} Milliseconds, time/repeat={2}", length, stopwatch.ElapsedMilliseconds, stopwatch.ElapsedMilliseconds / (float)length));
            Console.ReadKey();

            channel.ShutdownAsync().Wait();
        }
    }
}
