using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thrift.Collections;

using Thrift.Transport;
using Thrift.Protocol;
using Thrift.Server;

namespace App.ThriftDemoServer.Impl
{
    public class TradeServiceEventHandler : TServerEventHandler
    {
        public int callCount = 0;
        public object createContext(TProtocol input, TProtocol output)
        {
            //callCount++;
            return null;
        }

        public void deleteContext(object serverContext, TProtocol input, TProtocol output)
        {
            //callCount++;
        }

        public void preServe()
        {
            //callCount++;
        }

        public void processContext(object serverContext, TTransport transport)
        {
            callCount++;
            Console.WriteLine(callCount);
        }
    }
}
