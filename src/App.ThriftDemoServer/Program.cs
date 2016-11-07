using App.ThriftDemoServer.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using App.Thrift.Service;
using System.Security.Cryptography.X509Certificates;
using Thrift.Transport;
using Thrift.Protocol;
using Thrift.Server;
using App.ThriftDemo;
using App.RPC.Core;

namespace App.ThriftDemoServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Execute(args);
            Console.ReadLine();
        }

        public static bool Execute(string[] args)
        {
            try
            {
                bool useBufferedSockets = false, useFramed = false, useEncryption = false, compact = false, json = false;
                int port = 9090;
               
                string pipe = null;
                string certPath = "";
                var dic = Common.GetArgs(args);
                if (dic.ContainsKey("pipe"))
                {
                    pipe = dic["pipe"];
                }

                if (dic.ContainsKey("port"))
                {
                    port = int.Parse(dic["port"]);
                }

                if(dic.ContainsKey("b") || dic.ContainsKey("buffered"))
                {
                    useBufferedSockets = true;
                }

                if(dic.ContainsKey("f") || dic.ContainsKey("framed"))
                {
                    useFramed = true;
                }

                if (dic.ContainsKey("protocol"))
                {
                    compact = dic["protocol"] == "compact";
                    json= dic["protocol"] == "json";
                }

                if (dic.ContainsKey("ssl"))
                {
                    useEncryption = true;
                }

                if (dic.ContainsKey("cert"))
                {
                    certPath = dic["cert"];
                }



                // Processor
                DemoServiceImpl demoService = new DemoServiceImpl();
                RPCDemoService.Processor demoProcessor = new RPCDemoService.Processor(demoService);

                // Transport
                TServerTransport trans;
                if (pipe != null)
                {
         
                     trans = new TNamedPipeServerTransport(pipe);
                }
                else
                {
                    if (useEncryption)
                    {
                        trans = new TTLSServerSocket(port, 0, useBufferedSockets, new X509Certificate2(certPath));
                    }
                    else
                    {
                        trans = new TServerSocket(port, 0, useBufferedSockets);
                    }
                }

                TProtocolFactory proto;
                if (compact)
                    proto = new TCompactProtocol.Factory();
                else if (json)
                    proto = new TJSONProtocol.Factory();
                else
                    proto = new TBinaryProtocol.Factory();

                // Simple Server
                TServer serverEngine;
                if (useFramed)
                    serverEngine = new ThriftServer(demoProcessor, trans, new TFramedTransport.Factory(), proto);
                else
                    serverEngine = new ThriftServer(demoProcessor, trans, new TTransportFactory(), proto);

                // ThreadPool Server
                // serverEngine = new TThreadPoolServer(testProcessor, tServerSocket);

                // Threaded Server
                // serverEngine = new TThreadedServer(testProcessor, tServerSocket);

                //Server event handler
                //TradeServerEventHandler serverEvents = new TradeServerEventHandler();
                //serverEngine.setEventHandler(serverEvents);

               
                // Run it
                string where = (pipe != null ? "on pipe " + pipe : "on port " + port);
                Console.WriteLine("Starting the server " + where +
                    (useBufferedSockets ? " with buffered socket" : "") +
                    (useFramed ? " with framed transport" : "") +
                    (useEncryption ? " with encryption" : "") +
                    (compact ? " with compact protocol" : "") +
                    (json ? " with json protocol" : "") +
                    "...");
                //serverEngine.Run();
                serverEngine.Serve();
            }
            catch (Exception x)
            {
                Console.Error.Write(x);
                return false;
            }
            Console.WriteLine("done.");
            return true;
        }
    }
}
