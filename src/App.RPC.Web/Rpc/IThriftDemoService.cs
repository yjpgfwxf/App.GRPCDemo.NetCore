using App.Thrift.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.RPC.Web.Rpc
{
    public interface IThriftDemoService
    {
        DemoRequest GetById(int id);

        List<DemoRequest> Get(Search request);

        Response Add(DemoRequest request);
    }
}
