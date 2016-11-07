using App.RPC.Service;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace YCApp.RPCDemoClient
{
    public class Test
    {
        public static void TestSingleton(string host, long length)
        {

            string strGet = "", strAdd = "";

            Channel channel = new Channel(host, ChannelCredentials.Insecure);
            var client = new RPCDemoService.RPCDemoServiceClient(channel);

            TaskFactory factory = new TaskFactory();
            TaskFactory factoryAdd = new TaskFactory();

            List<Task> listTaskGet = new List<Task>();
            List<Task> listTaskAdd = new List<Task>();

            var stopwatch = Stopwatch.StartNew();

            var stopwatchAdd = Stopwatch.StartNew();

            for (var i = 0; i < length; i++)
            {
                var task = new Task(

                    () =>
                    {
                        var start = DateTime.Now;
                        var result = client.GetById(new DemoId() { Id = i });
                        var end = DateTime.Now;
                        Console.WriteLine(string.Format("time={0} Milliseconds,receive={1},", (end - start).TotalMilliseconds, JsonConvert.SerializeObject(result)));
                    }
                );
                listTaskGet.Add(task);

                var taskAdd = new Task(

                    () =>
                    {
                        Console.WriteLine("receive" + JsonConvert.SerializeObject(client.Add(new DemoRequest() { Id = DateTime.Now.Ticks.ToString(), CommentId = i })));
                    }
                    );

                listTaskAdd.Add(taskAdd);
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
                    Console.WriteLine(strAdd);
                }
                );

            factoryAdd.ContinueWhenAll(listTaskAdd.ToArray(),
                p =>
                {
                    stopwatchAdd.Stop();
                    strAdd = string.Format("call Add method length={0},time={1} Milliseconds, time/repeat={2}"
                  , length
                  , stopwatchAdd.ElapsedMilliseconds
                  , stopwatchAdd.ElapsedMilliseconds / (float)length);
                    Console.WriteLine(strAdd);
                    Console.WriteLine(strGet);
                }
                );

            listTaskGet.ForEach(p => p.Start());
            listTaskAdd.ForEach(p => p.Start());
        }


        public static void TestInstance(string host, long length)
        {
            string strGet = "", strAdd = "";
            Channel channel = new Channel(host, ChannelCredentials.Insecure);

            TaskFactory factory = new TaskFactory();
            TaskFactory factoryAdd = new TaskFactory();

            List<Task> listTaskGet = new List<Task>();
            List<Task> listTaskAdd = new List<Task>();

            var stopwatch = Stopwatch.StartNew();
            var stopwatchAdd = Stopwatch.StartNew();

            for (var i = 0; i < length; i++)
            {
                var task = new Task(
                    () =>
                    {

                        var client = new RPCDemoService.RPCDemoServiceClient(channel);
                        Console.WriteLine("receive" + JsonConvert.SerializeObject(client.GetById(new DemoId() { Id = i })));
                    }
                );
                listTaskGet.Add(task);

                var taskAdd = new Task(

                    () =>
                    {
                        var client = new RPCDemoService.RPCDemoServiceClient(channel);
                        Console.WriteLine("receive" + JsonConvert.SerializeObject(client.Add(new DemoRequest() { Id = DateTime.Now.Ticks.ToString(), CommentId = i })));
                    }
                    );

                listTaskAdd.Add(taskAdd);
            }

            factory.ContinueWhenAll(listTaskGet.ToArray(),
                p =>
                {
                    strGet = string.Format("instance: call GetById method length={0},time={1} Milliseconds, time/repeat={2}"
                  , length
                  , stopwatch.ElapsedMilliseconds
                  , stopwatch.ElapsedMilliseconds / (float)length);
                    Console.WriteLine(strGet);
                    Console.WriteLine(strAdd);
                }
                );

            factoryAdd.ContinueWhenAll(listTaskAdd.ToArray(),
                p =>
                    {
                        strAdd = string.Format("instance: call Add method length={0},time={1} Milliseconds, time/repeat={2}"
                      , length
                      , stopwatchAdd.ElapsedMilliseconds
                      , stopwatchAdd.ElapsedMilliseconds / (float)length);

                        Console.WriteLine(strAdd);
                        Console.WriteLine(strGet);
                        channel.ShutdownAsync().Wait();
                    }
                );

            listTaskGet.ForEach(p => p.Start());
            listTaskAdd.ForEach(p => p.Start());
        }

    }
}
