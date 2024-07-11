using System;
using Microsoft.Owin.Hosting;

namespace NancyHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:1234";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine($"Running on {url}");
                Console.ReadLine();
            }
        }
    }
}