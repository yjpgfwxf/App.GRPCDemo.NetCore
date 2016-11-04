# App.GRPCDemo.NetCore
1.配置packages\Grpc.Tools\1.0.0\tools\windows_x64\protoc.exe环境变量
开始运行cmd,转到 App.RPCDemo\proto 目录运行
--plugin=protoc-gen-grpc 值为grpc_csharp_plugin.exe文件的路径

protoc.exe --csharp_out=d:\grpcdemo\code\ --grpc_out=d:\grpcdemo\code\ --plugin=protoc-gen-grpc=C:\Users\liuyuhua\.nuget\packages\Grpc.Tools\1.0.0\tools\windows_x64\grpc_csharp_plugin.exe result.proto
protoc.exe --csharp_out=d:\grpcdemo\code\ --grpc_out=d:\grpcdemo\code\ --plugin=protoc-gen-grpc=C:\Users\liuyuhua\.nuget\packages\Grpc.Tools\1.0.0\tools\windows_x64\grpc_csharp_plugin.exe RPCDemoService.proto

把生成的文件copy到App.RPCDemo工程下 