using Hangfire;
using Hangfire.Server;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuto.Server
{
    internal class ScheduleServer
    {
        private static BackgroundJobServer _instance;
        private static BackgroundJobServerOptions _instanceOptions;


        public static void ConfigureServer(string CustomServerName = "localhost")
        {
            _instanceOptions = new BackgroundJobServerOptions
            {
                ServerName = CustomServerName
            };
        }


        // Configure server for monitoring Scheduled Tasks
        public static void StartServer()
        {
            ConfigureServer();
            using (_instance = new BackgroundJobServer(_instanceOptions))
            {
                Console.WriteLine($"Monitoring Server: {_instanceOptions.ServerName}");
                // stops the console application from immediately exiting
                Console.ReadKey();
                ShutdownServer();
            }
        }


        // gracefully stops the server
        public static void ShutdownServer()
        {
            _instance.Dispose();
        }
    }
}
