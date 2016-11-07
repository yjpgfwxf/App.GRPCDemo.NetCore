using App.Thrift.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ThriftDemoServer.Impl
{
    public class DemoServiceImpl : RPCDemoService.Iface
    {
        public Response Add(DemoRequest item)
        {
            
            return new Response { Message = "成功" + DateTime.Now.Ticks.ToString(), Sucess = true };
        }

        public List<DemoRequest> Get(Search search)
        {
            var result = new List<DemoRequest>();
            result.Add(new DemoRequest()
            {
                CommentId = 1,
                Id = DateTime.Now.Ticks.ToString(),
                IsDeleted = false
            });
            return result;
        }

        public DemoRequest GetById(int DemoId)
        {
            return new DemoRequest()
            {
                CommentId = DemoId,
                Id = DateTime.Now.Ticks.ToString(),
                IsDeleted = false
            };
        }
    }
}
