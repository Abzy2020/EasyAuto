using System;

public class Selection
{

    class InitialOptions
    {
        [Option('i', "initial", Required = true, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string InitialSetting { get; set; }
    }



    class EventOptions
    {

    }



    class ScheduleOptions
    {
        [Option('sT', "scheduleType", Required = true, HelpText = "Desired type of scheduled Automation (Once, DelayOnce, Date, Recurr, Remove, or List)")]
        public string InitialSetting { get; set; }
    }



    class OnceOptions
    {
        [Option('e', "executablePath", Required = true, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string executablePath { get; set; }

        [Option('aR', "arguments", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string arguments { get; set; }

        [Option('u', "useShellExecute", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string useShellExecute { get; set; }
    }



    class DelayOnceOptions
    {
        [Option('t', "time", Required = true, HelpText = "Designated time the automated task wil run")]
        public string time { get; set; }

        [Option('e', "executablePath", Required = true, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string executablePath { get; set; }

        [Option('aR', "arguments", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string arguments { get; set; }

        [Option('u', "useShellExecute", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string useShellExecute { get; set; }
    }



    class DelayOnceOptions
    {
        [Option('t', "time", Required = true, HelpText = "Designated time the automated task wil run (seconds, minutes, hours, days, months, or years)")]
        public string time { get; set; }

        [Option('e', "executablePath", Required = true, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string executablePath { get; set; }

        [Option('aR', "arguments", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string arguments { get; set; }

        [Option('u', "useShellExecute", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string useShellExecute { get; set; }
    }



    class DateOptions
    {
        [Option('d', "datetime", Required = true, HelpText = "Designated time the automated task wil run")]
        public DateTime dateTime { get; set; }

        [Option('e', "executablePath", Required = true, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string executablePath { get; set; }

        [Option('aR', "arguments", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string arguments { get; set; }

        [Option('u', "useShellExecute", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string useShellExecute { get; set; }
    }



    class RecurrOptions
    {
        [Option('e', "executablePath", Required = true, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string executablePath { get; set; }

        [Option('aR', "arguments", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string arguments { get; set; }

        [Option('u', "useShellExecute", Required = false, HelpText = "Select type of automation or help (help, schedule, or event)")]
        public string useShellExecute { get; set; }

        [Option('tI', "taskId", Required = false, HelpText = "Choose the name to identify the recurring task")]
        public string id { get; set; }

        [Option('c', "cron", Required = false, HelpText = "configure the cron schedule")]
        public string cron { get; set; }

        [Option('iS', "immediateStart", Required = false, HelpText = "Decides whether to start the task immediately after registering the task")]
        public string immediateStart { get; set; }

        [Option('r', "replace", Required = false, HelpText = "If a recurring task with the identifier already exists, it gets replaced.")]
        public string cron { get; set; }
    }



    class RemoveOption
    {
        [Option('tI', "taskId", Required = true, HelpText = "Specify the id of the task to be removed.")]
        public string taskId { get; set; }
    }

}
