using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Thrift.Service;
using Thrift.Protocol;
using Thrift.Transport;

namespace App.RPC.Web.Rpc
{
    public class ThriftDemoService : IThriftDemoService
    {
        private string Host
        {
            get
            {
                return AppConfig.GetAppSettings<string>("DemoServiceThriftHost");
            }
        }


        private int Port
        {
            get
            {
                return AppConfig.GetAppSettings<int>("DemoServiceThriftPort");
            }
        }

        private string ProtocolType
        {
            get
            {
                return AppConfig.GetAppSettings<string>("DemoServiceThriftProtocol");
            }
        }

        private TTransport Transport { get; set; }

        private TProtocol Protocol { get; set; }

        private RPCDemoService.Client Client { get; set; }

        public ThriftDemoService()
        {

            this.Transport = new TSocket(this.Host, this.Port);
            if (this.ProtocolType == "compact")
                this.Protocol = new TCompactProtocol(this.Transport);
            else if (this.ProtocolType == "json")
                Protocol = new TJSONProtocol(this.Transport);
            else
                Protocol = new TBinaryProtocol(this.Transport);
            this.Client = new RPCDemoService.Client(this.Protocol);

            if (!this.Transport.IsOpen)
            {
                this.Transport.Open();
            }
        }

        public Response Add(DemoRequest request)
        {
            return this.Client.Add(request);
        }

        public List<DemoRequest> Get(Search request)
        {
            return this.Client.Get(request);
        }

        public DemoRequest GetById(int id)
        {
            return this.Client.GetById(id);
        }
    }
}
