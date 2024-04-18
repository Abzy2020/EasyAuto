using Hangfire;
using Hangfire.SqlServer;

using CommandLine;
using CommandLine.Text;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using System.Diagnostics;
using System.Configuration;
using System.Collections;

using static EasyAuto.Scheduling.ScheduleManager;
using EasyAuto.Scheduling;
using EasyAuto.Events.CIM;
using EasyAuto.Events;
using EasyAuto.Server;
using EasyAuto.FlagOptions;
using EasyAuto.Events.Watcher.Network;
using System.Net.NetworkInformation;



/*
        
    TIME: 
        - Users can configure actions to trigger in the future based on date and time. 
        - They can also determine whether the actions should occur once or repeatedly using CRON scheduling.

    EVENTS:
        - Users can configure actions to trigger based on events happening in their system.
        - Potential usage can be ensurance of a desired state for one's system.

    ACTIONS:
        - allow for shell commands to be executed through the program
        - allow other applications to be executed
*/

namespace EasyAuto
{
    [SupportedOSPlatform("windows")]
    internal class Driver
    {
        //TODO: Develop driver that will take in user input for various automation requests

        // Ex: EasyAuto -S -DelayOnce -Second 10 powershell.exe Get-Process true

        /* Flags:
         * EasyAuto
         *      -Server: start server
                -H: print program manual
                -S: Schedule Based Automation
                    (frequency) -                                       [Once, DelayedOnce, FutureDate, Recurr]

                    -Once                                               [datetime]
                        - Program                                       [Executable filename]        
                            -Arguments                                  [program flags/arguments]
                                -ShellExecute                           [True/False]

                    -DelayOnce                                         
                        -time                                           [timesetting (0 (Second) | 1 (Minute) | 2 (Hour) | 3 (Day) | 4 (Month) | 5 (Year)) ]
                            - Program                                   [Executable filename]        
                                -Arguments                              [program flags/arguments]
                                    -ShellExecute                       [True/False]

                    -Date                                               [datetime]
                        - Program                                       [Executable filename]        
                            -Arguments                                  [program flags/arguments]
                                -ShellExecute                           [True/False]

                    -Recurr                                            
                        - Program                                       [Executable filename]        
                            -Arguments                                  [program flags/arguments]
                                -ShellExecute                           [True/False]
                                    -Id                                 [string]
                                        -cron                           [ex: * * * * *]
                                            -startafter creation        [True/False] false by default
                                                -remove if exists       [True/False] true by default

                    -Rm (Remove) - Remove a job                         [Id (1...n)]
                    
                    -L (List) - List jobs                               [*, Id (1...n)]
                        - specify states                                [Succeeded, Failed, Scheduled, Processing, Enqueued] 
        
                    
                -E: Event Based Automation

                    -Nt - Network watcher
                        - Program                                       [Executable filename]        
                            -Arguments                                  [program flags/arguments]
        
                    -Fs - Filesystem watcher
                        - Program                                       [Executable filename]        
                            -Arguments                                  [program flags/arguments]

                    -Br - Brightness watcher
                        - Program                                       [Executable filename]        
                            -Arguments                                  [program flags/arguments]

        */

        public enum TimeOptions
        {
            Seconds,
            Minutes,
            Hours,
            Days,
            Months,
            Years
        }
        
        static Dictionary<string, int> times = new Dictionary<string, int>();

        public static void Once(string program, string arguments, bool useShellExecute)
        {
            try
            {
                // initilize action object
                ScheduledAction action = new ScheduledAction(program, arguments, useShellExecute);
                // Run instruction Once
                GenerateEnqueuedTask(action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (program == null)
                {
                    Console.WriteLine("Please specify a program name.");
                }
                else if (arguments == null)
                {
                    Console.WriteLine("May have to specify commandline arguments. Please refer to program manual and/or documentation.");
                }
                else if (useShellExecute == null)
                {
                    Console.WriteLine("UseShellExecute options must be 'true' or 'false'.");
                }
            }
        }
        public static void Once(string program, bool useShellExecute)
        {
            string arguments = "";
            try
            {
                // initilize action object
                ScheduledAction action = new(program, arguments, useShellExecute);
                // Run instruction Once
                GenerateEnqueuedTask(action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (program == null)
                {
                    Console.WriteLine("Please specify a program name.");
                }
                else if (arguments == null)
                {
                    Console.WriteLine("May have to specify commandline arguments. Please refer to program manual and/or documentation.");
                }
                else if (useShellExecute != true | useShellExecute != false)
                {
                    Console.WriteLine("UseShellExecute options must be 'true' of 'false'.");
                }
            }
        }
        public static void DelayOnce(int timeSetting, int waitTime, string program, string arguments, bool useShellExecute)
        {
            try
            {
                // initilize action object
                ScheduledAction action = new(program, arguments, useShellExecute);
                // Run instruction Once
                GenerateDelayedTask(action, timeSetting, waitTime);
                Console.WriteLine("DelayOnce Action.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (program == null)
                {
                    Console.WriteLine("Please specify a program name.");
                }
                else if (arguments == null)
                {
                    Console.WriteLine("May have to specify commandline arguments. Please refer to program manual and/or documentation.");
                }
                else if (useShellExecute != true | useShellExecute != false)
                {
                    Console.WriteLine("UseShellExecute options must be 'true' of 'false'.");
                }
                else if (timeSetting != 0 || timeSetting != 1 || timeSetting != 2 || timeSetting != 3 || timeSetting != 4 || timeSetting != 5 || timeSetting != 6) 
                {
                    Console.WriteLine("Please provide the interval in which to measure the delay time.");
                }
                Console.WriteLine("Issue creating job");
            }
        }
        public static void DelayOnce(int timeSetting, int waitTime, string program, bool useShellExecute)
        {
            string arguments = "";
            try
            {
                // initilize action object
                ScheduledAction action = new(program, arguments, useShellExecute);
                // Run instruction Once
                GenerateDelayedTask(action, timeSetting, waitTime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (program == null)
                {
                    Console.WriteLine("Please specify a program name.");
                }
                else if (arguments == null)
                {
                    Console.WriteLine("May have to specify commandline arguments. Please refer to program manual and/or documentation.");
                }
                else if (useShellExecute != true | useShellExecute != false)
                {
                    Console.WriteLine("UseShellExecute options must be 'true' of 'false'.");
                }
                else if (timeSetting != 0 || timeSetting != 1 || timeSetting != 2 || timeSetting != 3 || timeSetting != 4 || timeSetting != 5 || timeSetting != 6)
                {
                    Console.WriteLine("Please provide the interval in which to measure the delay time.");
                }
            }
        }
        public static void Date(string schedule, string program, string arguments, bool useShellExecute)
        {
            try
            {
                // initilize action object
                ScheduledAction action = new(program, arguments, useShellExecute);
                // Run instruction after certain amount of time
                GenerateDateTimeTask(action, schedule);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (program == null)
                {
                    Console.WriteLine("Please specify a program name.");
                }
                else if (arguments == null)
                {
                    Console.WriteLine("May have to specify commandline arguments. Please refer to program manual and/or documentation.");
                }
                else if (useShellExecute != true | useShellExecute != false)
                {
                    Console.WriteLine("UseShellExecute options must be 'true' of 'false'.");
                }
            }
        }
        public static void Date(string schedule, string program, bool useShellExecute)
        {
            try
            {
                // initilize action object
                ScheduledAction action = new(program, "", useShellExecute);
                // Run instruction after certain amount of time
                GenerateDateTimeTask(action, schedule);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void Recur(string program, bool useShellExecute, string taskName, string cronSetting, bool startAfterCreation = false, bool removeIfExists = true)
        {
            try
            {
                // initilize action object
                ScheduledAction action = new(program, useShellExecute);
                // Run instruction after certain amount of time
                GenerateRecurrentTask(action, taskName, cronSetting, startAfterCreation, removeIfExists);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (program == null)
                {
                    Console.WriteLine("Please specify a program name.");
                }
                else if (useShellExecute != true | useShellExecute != false)
                {
                    Console.WriteLine("UseShellExecute options must be 'true' of 'false'.");
                }
                else if (taskName == null)
                {
                    Console.WriteLine("Please provide a name for the recurrent task");
                }
                else if (cronSetting == null)
                {
                    Console.WriteLine("Please provide a cron setting. Ex: * * * * *");
                }
            }
        }
        public static void Recur(string program, string? arguments, bool useShellExecute, string taskName, string cronSetting, bool startAfterCreation = false, bool removeIfExists = true)
        {
            try
            {
                // initilize action object
                ScheduledAction action = new(program, arguments, useShellExecute);
                // Run instruction after certain amount of time
                GenerateRecurrentTask(action, taskName, cronSetting, startAfterCreation, removeIfExists);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (program == null)
                {
                    Console.WriteLine("Please specify a program name.");
                }
                else if (arguments == null)
                {
                    Console.WriteLine("May have to specify commandline arguments. Please refer to program manual and/or documentation.");
                }
                else if (useShellExecute != true | useShellExecute != false)
                {
                    Console.WriteLine("UseShellExecute options must be 'true' of 'false'.");
                }
                else if (taskName == null)
                {
                    Console.WriteLine("Please provide a name for the recurrent task");
                }
                else if (cronSetting == null)
                {
                    Console.WriteLine("Please provide a cron setting. Ex: * * * * *");
                }
            }
        }


        private static void SchedulingConfig()
        {
            var options = new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = false,
                QueuePollInterval = TimeSpan.Zero,
                
            };
            // configure settings
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseSqlServerStorage(@"Server=ZBZY;Database=easyautodb;Trusted_Connection=True;", options)
                .WithJobExpirationTimeout(TimeSpan.FromSeconds(60));
            Console.WriteLine("Configured Global Settings.");
        }


        public static void PrintErrors(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }

        // decides which monitor to activate based on user specification
        // also passes program and arguments for automation
        public static void ChooseMonitor(Selection.Options monitorOptions, string executable, string? arguments)
        {
            switch (monitorOptions.MonitorSetting)
            {
                case "Network":
                    EventManager.CreateNetworkWatcher(executable, arguments);
                    break;
            }
        }


        static void Main(string[] args)
        {
            times["seconds"] = 0;
            times["minutes"] = 1;
            times["hours"] = 2;
            times["days"] = 3;
            times["months"] = 4;
            times["years"] = 5;


            // parsers 
            ParserResult<Selection.Options> result = Parser.Default.ParseArguments<Selection.Options>(args);
            var helpParser = new Parser(with => with.HelpWriter = Console.Out);

            if (args.Length == 0)
            {
                HelpText helpText = HelpText.AutoBuild(result, x => x, x => x);
                Console.WriteLine(helpText);
                Environment.Exit(0);
            }

            // sets the initial option for the program to run
            result.WithParsed(options => // parsed initial option successfully
                {
                    // initial options arguments
                    string initialSetting = options.InitialSetting;
                    string listSetting = options.ListSetting;
                    bool beginServer = options.BeginServer;

                    // Scheduling options
                    if (initialSetting == "schedule")
                    {
                        Console.WriteLine("Scheduling automation");

                        // make hangfire configuration
                        SchedulingConfig();

                        // scheduling options arguments
                        string scheduleType = options.ScheduleType;

                        if (scheduleType == "Once")
                        {
                            ParserResult<Selection.IOnceOptions> result = Parser.Default.ParseArguments<Selection.IOnceOptions>(args);
                            // if args are present
                            result.WithParsed(options =>
                            {
                                if ( options.Arguments != null)
                                {
                                    Once(options.ExecutablePath, options.Arguments, options.UseShellExecute);
                                } // if no args are present 
                                else
                                {
                                    Once(options.ExecutablePath, options.UseShellExecute);
                                }
                            });
                        }
                        else if (scheduleType == "DelayOnce")
                        {
                            ParserResult<Selection.IDelayOnceOptions> result = Parser.Default.ParseArguments<Selection.IDelayOnceOptions>(args);
                            result.WithParsed(delayOptions =>
                            {
                                if (options.Arguments != null)
                                {
                                    DelayOnce(times[delayOptions.Time], delayOptions.WaitTime, options.ExecutablePath, options.UseShellExecute);
                                }
                                else
                                {
                                    DelayOnce(times[delayOptions.Time], delayOptions.WaitTime, options.ExecutablePath, options.Arguments, options.UseShellExecute);
                                }
                            });
                        }
                        else if (scheduleType == "Date")
                        {
                            ParserResult<Selection.IDateOptions> result = Parser.Default.ParseArguments<Selection.IDateOptions>(args);
                            result.WithParsed(dateOptions =>
                            {
                                if (options.Arguments != null)
                                {
                                    Date(dateOptions.DateTime, options.ExecutablePath, options.UseShellExecute);
                                } else
                                {
                                    Date(dateOptions.DateTime, options.ExecutablePath, options.Arguments, options.UseShellExecute);
                                }
                            });
                        }
                        else if (scheduleType == "Recur")
                        {
                            ParserResult<Selection.IRecurOptions> result = Parser.Default.ParseArguments<Selection.IRecurOptions>(args);
                            result.WithParsed(recurOptions =>
                            {
                                if (options.Arguments != null)
                                {
                                    Recur(options.ExecutablePath, options.UseShellExecute, recurOptions.Id, recurOptions.Cron, recurOptions.ImmediateStart, recurOptions.Replace);
                                } else
                                {
                                    Recur(options.ExecutablePath, options.Arguments, options.UseShellExecute, recurOptions.Id, recurOptions.Cron, recurOptions.ImmediateStart, recurOptions.Replace);
                                }
                            });
                        }
                    }


                    // Event based options
                    else if (initialSetting == "monitor")
                    {
                        Console.WriteLine("Monitoring Automation");
                        ChooseMonitor(options, options.ExecutablePath, options.Arguments);
                    }


                    // Starts the server if this is true
                    else if (beginServer == true)
                    {
                        Console.WriteLine("Starting Server");
                        SchedulingConfig();
                        ScheduleServer.StartServer();
                    }


                    // lists jobs basde on status
                    else if (listSetting != null)
                    {
                        GetJobs(listSetting);
                    }


                }).WithNotParsed(PrintErrors);  // parsed initial option unsuccessfully

            Console.ReadKey();
        }
    }
}
