using RPCClient;

var rpcClient = new RpcClient();

Console.WriteLine(" [x] Requesting fib(30)");
var response = rpcClient.Call("100");

Console.WriteLine(" [.] Got '{0}'", response);
rpcClient.Close();

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();