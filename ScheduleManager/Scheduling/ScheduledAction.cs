using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management.Automation;
using Hangfire;


namespace EasyAuto.Scheduling
{
    [Serializable]
    public class ScheduledAction
    {
        public string? FileName;         // user specified program that the code will be executed by
        public string? Arguments;        // user specified command/code that will be executed
        public bool UseShellExecute;    // user specified option whether process should start from shell or directly from the exe

        public ScheduledAction()
        {
        }

        public ScheduledAction(string FileName, bool UseShellExecute)
        {
            this.FileName = FileName;
            this.UseShellExecute = UseShellExecute;
        }

        public ScheduledAction(string FileName, string Arguments, bool UseShellExecute)
        {
            this.FileName = FileName;
            this.Arguments = Arguments;
            this.UseShellExecute = UseShellExecute;
        }


        // start the application/script
        public void StartProcess(string fileName, string arguments, bool useShellExecute)
        {
            PrintDetails(fileName, arguments, useShellExecute);
            try
            {
                if (arguments == "" || arguments == null)

                {
                    ProcessStartInfo startInfo = new(fileName);
                    startInfo.UseShellExecute = useShellExecute;
                    Process.Start(startInfo);
                } else
                {
                    ProcessStartInfo startInfo = new(fileName, arguments);
                    startInfo.UseShellExecute = useShellExecute;
                    Process.Start(startInfo);
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine($"There was a problem trying to run {fileName}");
            }
        }


        public void PrintDetails(string fileName, string arguments, bool useShellExecute)
        {
            Console.WriteLine($"""
                FileName: {fileName}
                Arguments: {arguments}
                UseShellExecute: {useShellExecute}\n
                """);
            Console.WriteLine("");
        }
    }
}
