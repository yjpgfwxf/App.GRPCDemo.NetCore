# App.GRPCDemo.NetCore
通过grpc实现微服务，在Centos 7、docker中进行测试，hosting grpc 服务
测试时，防火墙需要开放相应端口

1.配置packages\Grpc.Tools\1.0.0\tools\windows_x64\protoc.exe环境变量
开始运行cmd,转到 App.RPCDemo\proto 目录运行
--plugin=protoc-gen-grpc 值为grpc_csharp_plugin.exe文件的路径

protoc.exe --csharp_out=d:\grpcdemo\code\ --grpc_out=d:\grpcdemo\code\ --plugin=protoc-gen-grpc=yourpath\.nuget\packages\Grpc.Tools\1.0.0\tools\windows_x64\grpc_csharp_plugin.exe result.proto
protoc.exe --csharp_out=d:\grpcdemo\code\ --grpc_out=d:\grpcdemo\code\ --plugin=protoc-gen-grpc=yourpath\.nuget\packages\Grpc.Tools\1.0.0\tools\windows_x64\grpc_csharp_plugin.exe RPCDemoService.proto

把生成的文件copy到App.RPCDemo工程下 

2.部署过程

  App.RPCDemoServer 服务端，参数说明
  host:主机ip 地址，默认0.0.0.0
  port:开放端口，默认9007

  部署过程，发布程序，运行：
  dotnet App.RPCDemoServer.dll host=192.168.4.37 port=9007
  centos 7.2 下，也可以在windows 下运行

	[demo@node139 App.RPCDemoServer]$ dotnet App.RPCDemoServer.dll host=192.168.190.139 port=9007
	Google Grpc Starting
	RPC server 192.168.190.139 listening on port 9007
	Rpc Service started. Press Ctrl+C to shut down.

  App.RPCDemoClient 客户端，循环2次，测试两调用所需要的时间，参数说明
  host:主机ip 地址，默认0.0.0.0
  port:开放端口，默认9007
  repeat：重复调用次数

  [demo@node139 App.RPCDemoClient]$ dotnet  App.RPCDemoClient.dll host=192.168.190.139 port=9007 repeat=100

3.docker 下测试
  跳转到App.RPCDemoServer项目路径下

	[demo@node139 App.RPCDemoServer]$ docker build -t grpcemoserver .
	Sending build context to Docker daemon 23.67 MB
	Step 1 : FROM microsoft/dotnet:1.0.1-runtime
	 ---> c0a30708437a
	Step 2 : COPY . /publish
	 ---> Using cache
	 ---> dad7441480f3
	Step 3 : WORKDIR /publish
	 ---> Using cache
	 ---> 11429a59af07
	Step 4 : EXPOSE 9007
	 ---> Using cache
	 ---> 517b063e476c
	Step 5 : CMD dotnet App.RPCDemoServer.dll
	 ---> Using cache
	 ---> db2c0debde34
	Successfully built db2c0debde34

	运行这个images,docker run 或docker service
	[demo@node139 App.RPCDemoServer]$ docker run --name rcpdemo -d -p 9008:9007 grpcemoserver
	b37db4b39a92256504dbe3a681d096782846bd60809f4442484c0a896067381d
	[demo@node139 App.RPCDemoServer]$ docker ps -a | grep grpc
	b37db4b39a92        grpcemoserver       "dotnet App.RPCDemoSe"   13 seconds ago      Up 7 seconds        0.0.0.0:9008->9007/tcp   rcpdemo

	在打开一个窗口运行:
	[demo@node139 App.RPCDemoClient]$ dotnet  App.RPCDemoClient.dll host=192.168.190.139 port=9008 repeat=100


