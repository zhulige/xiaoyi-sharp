using XiaoYiSharp;


class Program
{
    private static XiaoYiAgent _agent = new XiaoYiAgent();
    static async Task Main(string[] args)
    {
        _agent.OTAUrl = "";
        _agent.OnMessageEvent += Agent_OnMessageEvent;
        _agent.Start();
    }

    private static Task Agent_OnMessageEvent(string type, string state, string message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}