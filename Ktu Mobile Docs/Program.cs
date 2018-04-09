using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Ktu_Mobile_Docs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:6000")
                .UseStartup<Startup>()
                .Build();
    }
}