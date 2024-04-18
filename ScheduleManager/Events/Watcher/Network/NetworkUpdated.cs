using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace EasyAuto.Events.Watcher.Network
{
    internal class NetworkUpdated : UpdateWatcher
    {
        public NetworkUpdated(string filename, string? arguments)
            : base(filename, arguments)
        {
        }

        // shows that status of the network
        public static void NetworkChange_AvailabilityChangedCallback(object sender, NetworkAvailabilityEventArgs e)
        {

            if (e.IsAvailable)
            {
                Console.WriteLine($"Network Connection Status: Available");
            }
            else
            {
                Console.WriteLine($"Network Connection Status: Unavailable");
            }
        
        }

    }
}
