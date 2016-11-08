using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.RPC.Web
{
    public class ResultData
    {
        public bool Sucess { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }

        public string RequestTime { get; set; }

        public ResultData()
        {
            this.Sucess = true;
            this.Msg = "成功";
        }

    }
}
