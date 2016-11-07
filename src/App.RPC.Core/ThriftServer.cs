using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace App.RPC.Core
{
    public class ThriftServer : TSimpleServer, IServer
    {
        public ThriftServer(TProcessor processor, TServerTransport serverTransport)
            : base(processor, serverTransport)
        {

        }
        public ThriftServer(TProcessor processor, TServerTransport serverTransport, LogDelegate logDelegate)
            : base(processor, serverTransport, logDelegate)
        {

        }
        public ThriftServer(TProcessor processor, TServerTransport serverTransport, TTransportFactory transportFactory)
            : base(processor, serverTransport, transportFactory)
        {

        }
        public ThriftServer(TProcessor processor, TServerTransport serverTransport, TTransportFactory transportFactory, TProtocolFactory protocolFactory)
            : base(processor, serverTransport, transportFactory, protocolFactory)
        {

        }

        public Status State
        {
            get;

            set;
        }

        public override void Serve()
        {
            this.State = Status.Running;
            base.Serve();
        }

        public void Start()
        {
            this.Serve();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
