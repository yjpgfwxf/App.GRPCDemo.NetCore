namespace csharp App.Thrift.Service


struct Response{
    1:bool sucess,
	2:string message
}

struct Search{
     1: i32 page,
	 2: i32 size,
	 3: string query
}

struct DemoRequest{
	1: string Id,
	2: i32 CommentId,
	3: bool IsDeleted
}

service RPCDemoService{
	Response Add(1:DemoRequest item),
	DemoRequest GetById(1:i32 DemoId=10),
	list<DemoRequest> Get(1:Search search)
}