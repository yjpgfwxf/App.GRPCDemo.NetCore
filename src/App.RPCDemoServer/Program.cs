using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.RPC.Model;
using Grpc.Core;
using App.RPC.Service;
using App.RPCDemoServer.Impl;
using YCApp.RPCDemo;
using System.Threading;
using App.RPC.Core;

namespace App.RPCDemoServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string host = "0.0.0.0";
            int port = 9007;
            var dic = Common.GetArgs(args);
            if (dic != null && dic.Count > 0)
            {
                string tempHost;
                string tempPort;
                if (dic.TryGetValue("host", out tempHost))
                {
                    host = tempHost;
                }
                if (dic.TryGetValue("port", out tempPort))
                {
                    port = Convert.ToInt32(tempPort);
                }
            }

            GrpcServer server = new GrpcServer
            {
                Services = { RPCDemoService.BindService(new RPCDemoImpl()) },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };
            Console.WriteLine("Google Grpc Starting");
            foreach (var item in server.Ports)
            {
                Console.WriteLine(string.Format("RPC server {0} listening on port {1}", item.Host, item.Port));
            }
            server.Run();

        }

    }
}
