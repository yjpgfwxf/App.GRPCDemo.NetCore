using App.Thrift.Service;
using App.ThriftDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;
using Newtonsoft.Json;

namespace App.ThriftDemoClient
{
    public class Program
    {

        private static int numIterations = 1;
        private static string protocol = "";
        private static long length = 1;
        private static string type = "1";


        public static void Main(string[] args)
        {
            Execute(args);

            Console.WriteLine("continue .....");
            Console.ReadLine();
        }


        public static bool Execute(string[] args)
        {
            try
            {
                string host = "localhost";
                int port = 9091;
                string url = null, pipe = null;
                int numThreads = 1;
                bool buffered = false, framed = false, encrypted = false;
                string certPath = "";

                var dic = Common.GetArgs(args);

                if (dic.ContainsKey("url"))
                {
                    url = dic["url"];
                }

                if (dic.ContainsKey("n"))
                {
                    numIterations = int.Parse(dic["n"]);
                }

                if (dic.ContainsKey("pipe"))
                {
                    pipe = dic["pipe"];
                }

                if (dic.ContainsKey("host"))
                {
                    host = dic["host"];
                }

                if (dic.ContainsKey("port"))
                {
                    port = int.Parse(dic["port"]);
                }

                if (dic.ContainsKey("b") || dic.ContainsKey("buffered"))
                {
                    buffered = true;
                }

                if (dic.ContainsKey("f") || dic.ContainsKey("framed"))
                {
                    framed = true;
                }

                if (dic.ContainsKey("t"))
                {
                    numThreads = int.Parse(dic["t"]);
                }

                if (dic.ContainsKey("protocol"))
                {
                    protocol = dic["protocol"];
                }

                if (dic.ContainsKey("ssl"))
                {
                    encrypted = true;
                }

                if (dic.ContainsKey("cert"))
                {
                    certPath = dic["cert"];
                }

                if (dic.ContainsKey("repeat"))
                {
                    length = Convert.ToInt64(dic["repeat"]);
                }

                if (dic.ContainsKey("type"))
                {
                    type = dic["type"];
                }


                ClientThread(host, port, certPath, url, pipe, encrypted, buffered, framed);
            }
            catch (Exception outerEx)
            {
                Console.WriteLine(outerEx.Message + " ST: " + outerEx.StackTrace);
                return false;
            }

            Console.WriteLine();
            Console.WriteLine();
            return true;
        }

        public static void ClientThread(string host, int port, string certPath, string url, string pipe, bool encrypted, bool buffered, bool framed)
        {
           
            switch (type)
            {
                case "2":
                    Test.TestInstance(host, port, certPath, url, pipe, encrypted, buffered, framed, protocol, length);
                    break;
               // case "3":
                    //Test.TestSingleton(transport, protocol, length);
                    //break;
                default:
                    ClientTest(host, port, certPath, url, pipe, encrypted, buffered, framed);
                    break;
            }
            //
        }

        public static void ClientTest(string host, int port, string certPath, string url, string pipe, bool encrypted, bool buffered, bool framed)
        {
            TTransport trans = null;

            if (url == null)
            {
                // endpoint transport

                if (pipe != null)
                    trans = new TNamedPipeClientTransport(pipe);
                else
                {
                    if (encrypted)
                        trans = new TTLSSocket(host, port, certPath);
                    else
                        trans = new TSocket(host, port);
                }

                // layered transport
                if (buffered)
                    trans = new TBufferedTransport(trans as TStreamTransport);
                if (framed)
                    trans = new TFramedTransport(trans);

                //ensure proper open/close of transport
                trans.Open();
            }
            else
            {
                trans = new THttpClient(new Uri(url));

            }


            TProtocol proto;
            if (protocol == "compact")
                proto = new TCompactProtocol(trans);
            else if (protocol == "json")
                proto = new TJSONProtocol(trans);
            else
                proto = new TBinaryProtocol(trans);

            RPCDemoService.Client client = new RPCDemoService.Client(proto);
            try
            {
                if (!trans.IsOpen)
                {
                    trans.Open();
                }
            }
            catch (TTransportException ttx)
            {
                Console.WriteLine("Connect failed: " + ttx.Message);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < length; i++)
            {

                var reply = client.GetById(i);
                Console.WriteLine("receive" + JsonConvert.SerializeObject(reply));
            }
            stopwatch.Stop();

            Console.WriteLine(string.Format("repeat={0}, time={1} Milliseconds, time/repeat={2}", length, stopwatch.ElapsedMilliseconds, stopwatch.ElapsedMilliseconds / (float)length));
            Console.ReadKey();

        }
    }
}
