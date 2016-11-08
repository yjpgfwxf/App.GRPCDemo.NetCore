using App.Thrift.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;

namespace App.ThriftDemoClient
{
    public class Test
    {
        public static void ClientTest(string host, int port, string certPath, string url, string pipe, bool encrypted, bool buffered, bool framed, string protocol, int i)
        {
            TTransport trans = null;
            if (url == null)
            {
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

            var reply = client.GetById(i);
            Console.WriteLine("receive" + JsonConvert.SerializeObject(reply));
            trans.Close();

        }

        public static void TestInstance(string host, int port, string certPath, string url, string pipe, bool encrypted, bool buffered, bool framed,string protocol,long length)
        {
            string strGet = "";

            TaskFactory factory = new TaskFactory();
           
            List<Task> listTaskGet = new List<Task>();
          
            var stopwatch = Stopwatch.StartNew();
         

            for (var i = 0; i < length; i++)
            {
                var task = new Task(
                    () =>
                    {
                        ClientTest(host, port, certPath, url, pipe, encrypted, buffered, framed, protocol, i);
                    }
                );
                listTaskGet.Add(task);
            }

            factory.ContinueWhenAll(listTaskGet.ToArray(),
              p =>
              {
                  stopwatch.Stop();
                  strGet = string.Format("call GetById method length={0},time={1} Milliseconds, time/repeat={2}"
                , length
                , stopwatch.ElapsedMilliseconds
                , stopwatch.ElapsedMilliseconds / (float)length);
                  Console.WriteLine(strGet);
              }
              );

            listTaskGet.ForEach(p => p.Start());
          
        }

    }
}
