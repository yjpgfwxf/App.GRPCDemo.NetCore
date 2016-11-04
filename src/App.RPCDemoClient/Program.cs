using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App.RPC.Model;
using Grpc.Core;
using App.RPC.Service;

using Newtonsoft.Json;
using YCApp.RPCDemo;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace YCApp.RPCDemoClient
{
    public class Program
    {
        public static void Main(string[] args)
        {

         

            string host = "127.0.0.1";
            string port = "9007";
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
            }

            Channel channel = new Channel(string.Format("{0}:{1}", host, port), ChannelCredentials.Insecure);
            var client = new RPCDemoService.RPCDemoServiceClient(channel);
            client.GetById(new DemoId() { Id = 6 });
            for (var j = 0; j < 2; j++)
            {

                var stopwatch = Stopwatch.StartNew();
                for (var i = 0; i < length; i++)
                {
                    
                    var reply = client.GetById(new DemoId() { Id = i });
                    Console.WriteLine("receive" + JsonConvert.SerializeObject(reply));
                }
                stopwatch.Stop();

                Console.WriteLine(string.Format("repeat={0}, time={1} Milliseconds, time/repeat={2}", length, stopwatch.ElapsedMilliseconds, stopwatch.ElapsedMilliseconds / (float)length));
                Console.ReadKey();
            }
            channel.ShutdownAsync().Wait();

        }
    }
}
