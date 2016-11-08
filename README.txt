1.跳转到App.ThriftDemo\thrift 目录下执行
  thrift-0.9.3 -r --gen csharp DemoService.thrift
2.编译项目并发布运行服务端
  dotnet App.ThriftDemoServer.dll protocol=compact port=9090

3.运行客户端
  dotnet App.ThriftDemoClient.dll protocol=compact port=9090 host=localhost repeat=10000