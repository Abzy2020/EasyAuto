using System;

using CommandLine;



namespace EasyAuto.FlagOptions
{
    internal class Selection
    {
        public interface IInitialOptions
        {
            [Option('o', "operation", Required = false, HelpText = "Select type of operation (schedule or monitor).")]
            string InitialSetting { get; set; }

            [Option('b', "beginServer", Required = false, HelpText = "Starts the server. Scheduled operations require the server to be active in order to process them.")]
            bool BeginServer { get; set; }
            
            [Option('l', "listSetting", Required = false, HelpText = "List jobs based on status (Succeeded, Failed, Scheduled, Processing, or Enqueued).")]
            string ListSetting { get; set; }
        }



        public interface IMonitorOptions
        {
            [Option('m', "monitor", Required = false, HelpText = "Specify an event watcher (Network).\nOnly specified in \"monitor\" operations.")]
            string MonitorSetting { get; set; }
        }



        public interface IScheduleOptions
        {
            [Option('s', "scheduleType", HelpText = "Desired type of scheduled Automation (Once, DelayOnce, Date, Recurr, Remove, or List).\nOnly specified in \"schedule\" operations.")]
            string ScheduleType { get; set; }
        }



        public interface IOnceOptions
        {
            [Option('e', "executablePath", Required = false, HelpText = "The path to the program or script that will be automated.")]
            string ExecutablePath { get; set; }

            [Option('a', "arguments", Required = false, HelpText = "Specifies any arguments for the script or program to be executed.")]
            string? Arguments { get; set; }

            [Option('u', "useShellExecute", HelpText = "If true, operating system handels how to run the program or script.")]
            bool UseShellExecute { get; set; }
        }



        public interface IDelayOnceOptions
        {
            [Option('t', "time", HelpText = "Time metric to use to wait for the task to run (seconds, minutes, hours, days, months, or years).\nOnly specified in \"DelayOnce\" schedules.", SetName = "delay")]
            string Time { get; set; }

            [Option('w', "waitTime", HelpText = "Time value to wait for task to run.\nOnly specified in \"DelayOnce\" schedules.", SetName = "delay")]
            int WaitTime { get; set; }
        }



        public interface IDateOptions
        {
            [Option('d', "datetime", HelpText = "Designated date/time the automated task will run.\nOnly specified in \"Date\" schedules.", SetName = "date")]
            string DateTime { get; set; }
        }



        public interface IRecurOptions
        {
            [Option('i', "id", HelpText = "Specifies the name to identify the recurring task.\nOnly specified in \"Recur\" schedules.", SetName = "recur")]
            string Id { get; set; }

            [Option('c', "cron", HelpText = "configure the cron schedule.\nOnly specified in \"Recur\" schedules.", SetName = "recur")]
            string Cron { get; set; }

            [Option("immediateStart", Default = false, HelpText = "Decides whether to start the task immediately after registering the task.\nOnly specified in \"Recur\" schedules.", SetName = "recur")]
            bool ImmediateStart { get; set; }

            [Option('r', "replace", Default = false, HelpText = "If a recurring task with the identifier already exists, it gets replaced.\nOnly specified in \"Recur\" schedules.", SetName = "recur")]
            bool Replace { get; set; }
        }



        public interface IRemoveOption
        {
            [Option('i', "taskId", HelpText = "Specify the id of the task to be removed.\nOnly specified in \"Remove\" operations.", SetName = "remove")]
            string TaskId { get; set; }
        }

        [Verb("monitor", HelpText = "For event based automation.")]
        public class Options : IInitialOptions, IScheduleOptions, IMonitorOptions,
            IOnceOptions, IDelayOnceOptions, IDateOptions, IRecurOptions, IRemoveOption
        {
            // initial
            public string InitialSetting { get; set; }
            public bool BeginServer { get; set; }
            public string ListSetting { get; set; }
            
            // schedule
            public string ScheduleType { get; set; }
            
            // monitor
            public string MonitorSetting {  get; set; }
            
            // not mutually exclusive
            public string ExecutablePath { get; set; }
            public string? Arguments { get; set; }
            public bool UseShellExecute { get; set; }
            
            // delay
            public string Time { get; set; }
            public int WaitTime { get; set; }
            
            // date
            public string DateTime { get; set; }
            
            // recur
            public string Id { get; set; }
            public string Cron { get; set; }
            public bool ImmediateStart { get; set; }
            public bool Replace { get; set; }
            public string TaskId { get; set; }
        }
    }

}
