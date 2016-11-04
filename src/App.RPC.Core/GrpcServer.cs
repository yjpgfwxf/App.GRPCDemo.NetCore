using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.RPC.Core
{
    public class GrpcServer : Server, IServer
    {
        public Status State
        {
            get;
            set;
        }

        public new void Start()
        {
            this.State = Status.Running;
            base.Start();
        }

        public void Stop()
        {
            this.ShutdownAsync().Wait();
        }
    }
}
