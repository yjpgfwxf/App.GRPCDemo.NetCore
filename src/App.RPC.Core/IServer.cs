using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.RPC.Core
{
    public interface IServer
    {
        void Start();

        void Stop();

        Status State { get; set; }
    }

    public enum Status
    {
        None,
        Stop,
        Running

    }
}
