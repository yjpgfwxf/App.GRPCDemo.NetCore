using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.RPC.Core
{
    public static class RpcHostExtensions
    {
        public static void Run(this IServer server)
        {
            var done = new ManualResetEventSlim(false);
            using (var cts = new CancellationTokenSource())
            {
                Action shutdown = () =>
                {
                    if (!cts.IsCancellationRequested)
                    {
                        server.Stop();
                        Console.WriteLine("Rpc Service is shutting down...");
                        cts.Cancel();
                    }
                    done.Wait();
                };

#if NETSTANDARD1_5
                var assemblyLoadContext = AssemblyLoadContext.GetLoadContext(typeof(WebHostExtensions).GetTypeInfo().Assembly);
                assemblyLoadContext.Unloading += context => shutdown();
#endif
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    shutdown();
                    // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                    eventArgs.Cancel = true;
                };

                server.Run(cts.Token, "Rpc Service started. Press Ctrl+C to shut down.");
                done.Set();
            }
        }

        /// <summary>
        /// Runs a web application and block the calling thread until token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="host">The <see cref="IWebHost"/> to run.</param>
        /// <param name="token">The token to trigger shutdown.</param>
        public static void Run(this IServer server, CancellationToken token)
        {
            server.Run(token, shutdownMessage: null);
        }

        private static void Run(this IServer server, CancellationToken token, string shutdownMessage)
        {
            if (server.State != Status.Running)
            {
                server.Start();
            }

            var applicationLifetime = new ApplicationLifetime();
            if (!string.IsNullOrEmpty(shutdownMessage))
            {
                Console.WriteLine(shutdownMessage);
            }

            token.Register(state =>
            {
                ((ApplicationLifetime)state).StopApplication();
            },
                applicationLifetime);

            applicationLifetime.ApplicationStopping.WaitHandle.WaitOne();

        }
    }
}
