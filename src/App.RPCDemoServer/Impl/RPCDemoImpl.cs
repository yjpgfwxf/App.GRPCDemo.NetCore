
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.RPC.Model;
using Grpc.Core;
using App.RPC.Service;

namespace App.RPCDemoServer.Impl
{
    public class RPCDemoImpl : RPCDemoService.RPCDemoServiceBase
    {
        public override Task<Response> Add(DemoRequest request, ServerCallContext context)
        {
           
            return Task.FromResult(new Response { Message = "成功" + context.Host + DateTime.Now.Ticks.ToString(), Sucess = true });
        }

        public override Task<DemoList> Get(Search request, ServerCallContext context)
        {
            var result = new DemoList();
            result.Details.Add(new DemoRequest()
            {
                CommentId = 1,
                Id = DateTime.Now.Ticks.ToString(),
                IsDeleted = false
            });
            return Task.FromResult(result);

        }

        public override Task<DemoRequest> GetById(DemoId request, ServerCallContext context)
        {
            return Task.FromResult(new DemoRequest()
            {
                CommentId = request.Id,
                Id = DateTime.Now.Ticks.ToString(),
                IsDeleted = false
            });
        }
    }
}
