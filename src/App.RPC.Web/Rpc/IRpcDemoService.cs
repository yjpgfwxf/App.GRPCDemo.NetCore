using App.RPC.Model;
using App.RPC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.RPC.Web.Rpc
{
    public interface IRpcDemoService
    {
        DemoRequest GetById(DemoId request);

        DemoList Get(Search request);

        Response Add(DemoRequest request);
    }
}
