using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.RPC.Web.Rpc;
using App.Thrift.Service;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace App.RPC.Web.Controllers
{
    [Route("api/[controller]")]
    public class ThriftDemoController : Controller
    {

        public IThriftDemoService Service { get; set; }

        public ThriftDemoController(IThriftDemoService service)
        {
            this.Service = service;
        }

        // GET: api/values
        [HttpGet]
        public ResultData Get(Search search)
        {
            DateTime start = DateTime.Now;
            ResultData result = new ResultData();
            result.Data = this.Service.Get(search);
            DateTime end = DateTime.Now;
            result.RequestTime = string.Format("{0}-{1}={2}Milliseconds", end.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), start.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), (end - start).TotalMilliseconds);
            return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ResultData Get(int id)
        {
            DateTime start = DateTime.Now;
            ResultData result = new ResultData();
            result.Data = this.Service.GetById(id);
            DateTime end = DateTime.Now;
            result.RequestTime = string.Format("{0}-{1}={2}Milliseconds", end.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), start.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), (end - start).TotalMilliseconds);
            return result;
        }

        // POST api/values
        [HttpPost]
        public ResultData Post(DemoRequest request)
        {
            DateTime start = DateTime.Now;
            ResultData result = new ResultData();
            result.Data = this.Service.Add(request);
            DateTime end = DateTime.Now;
            result.RequestTime = string.Format("{0}-{1}={2}Milliseconds", end.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), start.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), (end - start).TotalMilliseconds);
            return result;
        }

     
    }
}
