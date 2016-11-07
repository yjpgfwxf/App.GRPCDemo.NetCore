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
        private static long length = 10;

        public static void Main(string[] args)
        {
            Execute(args);
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
                string certPath = "../../../../../keys/server.pem";


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

                //issue tests on separate threads simultaneously
                Thread[] threads = new Thread[numThreads];
                DateTime start = DateTime.Now;
                for (int test = 0; test < numThreads; test++)
                {
                    Thread t = new Thread(new ParameterizedThreadStart(ClientThread));
                    threads[test] = t;
                    if (url == null)
                    {
                        // endpoint transport
                        TTransport trans = null;
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
                        t.Start(trans);
                    }
                    else
                    {
                        THttpClient http = new THttpClient(new Uri(url));
                        t.Start(http);
                    }
                }

                for (int test = 0; test < numThreads; test++)
                {
                    //threads[test].Join();
                }
                Console.Write("Total time: " + (DateTime.Now - start));
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

        public static void ClientThread(object obj)
        {
            TTransport transport = (TTransport)obj;
            for (int i = 0; i < numIterations; i++)
            {
                ClientTest(transport);
            }
            transport.Close();
        }

        public static void ClientTest(TTransport transport)
        {
            TProtocol proto;
            if (protocol == "compact")
                proto = new TCompactProtocol(transport);
            else if (protocol == "json")
                proto = new TJSONProtocol(transport);
            else
                proto = new TBinaryProtocol(transport);

            RPCDemoService.Client client = new RPCDemoService.Client(proto);
            try
            {
                if (!transport.IsOpen)
                {
                    transport.Open();
                }
            }
            catch (TTransportException ttx)
            {
                Console.WriteLine("Connect failed: " + ttx.Message);
                return;
            }
            client.GetById(1);
            for (var j = 0; j < 2; j++)
            {

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
}
