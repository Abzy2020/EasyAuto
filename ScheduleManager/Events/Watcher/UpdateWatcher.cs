using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuto.Events.Watcher
{


    abstract class UpdateWatcher
    {
        private string filename;
        private string? arguments;


        public UpdateWatcher(string filename, string? arguments)
        {
            this.filename = filename;
            this.arguments = arguments;
        }


        // allows user to automate a response to the AvailabilityChangedCallback event
        public void Response(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine($"Responding with {filename} {arguments}");
                ProcessStartInfo startInfo;

                if (arguments == null)
                {
                    startInfo = new() { FileName = filename };
                } 
                else
                {
                    startInfo = new(){FileName = filename, Arguments = arguments};
                }

                Process process = Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
