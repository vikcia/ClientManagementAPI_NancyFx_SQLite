using Nancy;

namespace NancyHost
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ => "Welcome to Nancy.";
            Get["/Test"] = _ => "Test Message.";
            Get["/Hello"] = _ => $"Hello {Request.Query["name"]}";
        }
    }
}