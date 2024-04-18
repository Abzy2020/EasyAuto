using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.Versioning;

using Microsoft.Management.Infrastructure;
using Microsoft.PowerShell.Commands;
using EasyAuto.Events.CIM;

using System.Net.NetworkInformation;
using EasyAuto.Events.Watcher.Network;

namespace EasyAuto.Events
{
    // For working with event based automation
    // - ProcessStartUp, ProcessShutdown, Battery, Network, FileSystem, Brightness, LogOn, StartUp, Mouse Movements, Keyboard typing, Sleep

    // Background Job Event Registration
    // - __EventFilter 
    // - CommandLineEventConsumer
    // - __FilterToConsumerBinding

    // - EventManager:
    // - *Abstract* CimInstance:
    //   - [what you want to automate (FileSystem, Battery, Brightness,...)]
    //   - Important Properties

    [SupportedOSPlatform("windows")]
    internal class EventManager
    {
        /*static void Main()
        {
            GetProcessInfo();
        }*/


        // Gets information on all currently running processes
        public static void GetProcessInfo()
        {
            ManagementClass cimClass = new("Win32_Process");
            ManagementObjectCollection cimInstances = cimClass.GetInstances();

            foreach (var instance in cimInstances)
            {
                PropertyDataCollection classProperties = instance.Properties;
                foreach (var property in classProperties)
                {
                    if (property.Name == "Name")
                    {
                        Console.WriteLine($"{property.Name}: {property.Value}");
                    }
                }
            }
        }


        // create a watcher for network related events
        public static void CreateNetworkWatcher(string filename, string? arguments)
        {
            NetworkUpdated n = new(filename, arguments);
            // subscribe to NetworkAvailabilityChanged
            NetworkChange.NetworkAvailabilityChanged += NetworkUpdated.NetworkChange_AvailabilityChangedCallback;
            NetworkChange.NetworkAvailabilityChanged += n.Response;

            Console.WriteLine($"Network watcher Created:\nEvent Response: {filename} {arguments}\n");
        }


        // create a watcher by providing a filter and a consumer
        public static void CreateBackgroundEventWatcher(string FilterName, string FilterDescription, 
            string TargetClassName, string ConsumerName, string CommandLineTemplate, string poll, string property, string conditionValue)
        {
            // create CIM instances
            CimFilter filter = new CimFilter(FilterName, FilterDescription);
            CimConsumer consumer = new CimConsumer(ConsumerName, CommandLineTemplate);
            CimBinder binder = new CimBinder();

            // configure the filter
            filter.BuildQuery(poll, TargetClassName);
            filter.RegisterFilter();

            // configure the consumer
            consumer.RegisterConsumer();

            // bind the filter to the consumer
            binder.CreateInstanceBinder(filter, consumer);
            Console.WriteLine($"Created Event Watcher:\nFilterName: {FilterName},\nFilterDescription: {FilterDescription},\nConsumerName: {ConsumerName}");
        }


        public static void DeleteBackgroundEventWatcher(string filterName, string consumerName)
        {
            // Delete __FilterToConsumerBinding
            ManagementObjectSearcher bindingSearcher = new ManagementObjectSearcher(
                @"root\subscription", $"SELECT * FROM __FilterToConsumerBinding WHERE Filter = '{filterName}' AND Consumer = '{consumerName}'");
            foreach (ManagementObject binding in bindingSearcher.Get())
            {
                binding.Delete();
            }

            // Delete __EventFilter
            ManagementObjectSearcher filterSearcher = new ManagementObjectSearcher(
                @"root\subscription", $"SELECT * FROM __EventFilter WHERE Name = '{filterName}'");
            foreach (ManagementObject filter in filterSearcher.Get())
            {
                filter.Delete();
            }

            // Delete __EventConsumer
            ManagementObjectSearcher consumerSearcher = new ManagementObjectSearcher(
                @"root\subscription", $"SELECT * FROM __EventConsumer WHERE Name = '{consumerName}'");
            foreach (ManagementObject consumer in consumerSearcher.Get())
            {
                consumer.Delete();
            }
        }
    }
}
