using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.RPC.Model;
using App.RPC.Service;
using Grpc.Core;
using static App.RPC.Service.RPCDemoService;

namespace App.RPC.Web.Rpc
{
    public class RpcDemoService : IRpcDemoService
    {
        private string Address
        {
            get
            {
                return AppConfig.GetAppSettings<string>("DemoService");
            }
        }

        private Channel Channel
        {
            get;set;
        }

        private RPCDemoServiceClient Client
        {
            get;set;
        }

        public RpcDemoService()
        {
            this.Channel = new Channel(Address, ChannelCredentials.Insecure);

            this.Client = new RPCDemoServiceClient(this.Channel);

        }

        public Response Add(DemoRequest request)
        {
            return this.Client.Add(request);
        }

        public DemoList Get(Search request)
        {
            return this.Client.Get(request);
        }

        public DemoRequest GetById(DemoId request)
        {
            return this.Client.GetById(request);
        }
    }
}
