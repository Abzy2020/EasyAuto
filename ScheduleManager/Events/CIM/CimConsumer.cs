using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuto.Events.CIM
{
    [SupportedOSPlatform("windows")]
    internal class CimConsumer : CimInstance
    {
        public new string Name;                                             // name of consumer
        public string CommandLineTemplate;                                  // path to the executable/script
        readonly ManagementClass __CommandLineEventConsumer;                // __EventConsumer class
        private ManagementObject __CommandLineEventConsumerInstance;        // __EventConsumer class instance


        public CimConsumer(string Name, string CommandLineTemplate) : base(@"root\subscription:CommandLineEventConsumer")
        {
            this.Name = Name;
            //set command line template
            this.CommandLineTemplate = CommandLineTemplate;
            // set working directory
            // filter class to narrow events
            //__CommandLineEventConsumer = new ManagementClass(@"root\subscription:CommandLineEventConsumer");
            // name of class we want to automate
            //__CommandLineEventConsumerInstance = __CommandLineEventConsumer.CreateInstance();
        }



        // configures settings for what should execute when the event is triggered
        public void RegisterConsumer()
        {
            // configure desired properties
            //__CommandLineEventConsumerInstance["CommandLineTemplate"] = CommandLineTemplate;
            //__CommandLineEventConsumerInstance["Name"] = Name;
            //__CommandLineEventConsumerInstance.Put();

            cimInstance["CommandLineTemplate"] = CommandLineTemplate;
            cimInstance["Name"] = Name;
            cimInstance["CreateNewConsole"] = true;
            cimInstance["RunInteractively"] = true;
            cimInstance["ShowWindowCommand"] = 1;
            cimInstance.Put();
        }


    }
}
