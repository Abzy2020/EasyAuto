using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Management.Automation;
using System.Security;
using System.Text;
using static EasyAuto.Scheduling.ScheduleManager;



namespace EasyAuto.Scheduling
{
    // For working with Automation determined by time
    public class ScheduleManager
    {
        private const string _connectionString = @"Server=ZBZY;Database=easyautodb;Trusted_Connection=True;";

        // used for setting schedules for tasks using dates and times
        public enum TimeSetting
        {
            Second,
            Minute,
            Hour,
            Day,
            Month,
            Year
        }


        // Add a task that will be executed immediately when it's turn comes in the queue
        public static void GenerateEnqueuedTask(ScheduledAction action)
        {
            string job = BackgroundJob.Enqueue(
                () => action.StartProcess(action.FileName, action.Arguments, action.UseShellExecute));
            Console.WriteLine($"JobId: {job}");
        }


        // Create a task that will be executed in the future relative to the current time
        public static void GenerateDelayedTask(ScheduledAction action, int timeSetting, int waitDuration)
        {
            switch (timeSetting)
            {
                // start task # SECONDS from now
                case (int)TimeSetting.Second:
                    BackgroundJob.Schedule(
                        () => action.StartProcess(action.FileName, action.Arguments, action.UseShellExecute),
                        TimeSpan.FromSeconds(waitDuration));
                    break;

                // start task # MINUTES from now
                case (int)TimeSetting.Minute:
                    BackgroundJob.Schedule(
                        () => action.StartProcess(action.FileName, action.Arguments, action.UseShellExecute),
                        TimeSpan.FromMinutes(waitDuration));
                    break;

                // start task # HOURS from now
                case (int)TimeSetting.Hour:
                    BackgroundJob.Schedule(
                        () => action.StartProcess(action.FileName, action.Arguments, action.UseShellExecute),
                        TimeSpan.FromHours(waitDuration));
                    break;

                // start task # DAYS from now
                case (int)TimeSetting.Day:
                    BackgroundJob.Schedule(
                        () => action.StartProcess(action.FileName, action.Arguments, action.UseShellExecute),
                        TimeSpan.FromDays(waitDuration));
                    break;
            }
        }


        // Create a task that will be executed using a specific date/time
        public static void GenerateDateTimeTask(ScheduledAction action, string schedule)
        {
            // set the scheduled time the action should be fired
            var datetime = DateTimeOffset.Parse(schedule);
            Console.WriteLine(datetime);

            // configure and start the job
            BackgroundJob.Schedule(
                () => action.StartProcess(action.FileName, action.Arguments, action.UseShellExecute), datetime);
        }


        // Create a task that will recurr based on an interval
        public static void GenerateRecurrentTask(ScheduledAction action, string jobId, string cronSchedule, bool startAfterCreation, bool removeIfExists)
        {
            RecurringJobManager recurringJob = new RecurringJobManager();

            // if the user wants to replace jobs that have a specific Id
            if (removeIfExists) { RemoveRecurrentTask(jobId); }

            // if the user wants to start the job immediately after it is created
            if (startAfterCreation) { recurringJob.Trigger(jobId); }

            recurringJob.AddOrUpdate(jobId,
                () => action.StartProcess(action.FileName, action.Arguments, action.UseShellExecute), cronSchedule);
        }


        // removes job from recurrent tasks
        public static void RemoveRecurrentTask(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }


        // gets all currently scheduled jobs
        public static void GetJobs(string type)
        {
            SqlConnection _connection = new(_connectionString);
            _connection.Open();
            SqlCommand cmd = new($"SELECT * FROM easyautodb.HangFire.Job WHERE StateName = '{type}'", _connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(string.Format("{0} | {1} | {2}", reader["id"], reader["InvocationData"], reader["Arguments"]));
                }
            }
            _connection.Close();
        }


        // gets all registered jobs
        public static void GetAllJobs()
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var jobs = monitoringApi.ScheduledJobs(0, int.MaxValue);

            foreach (var job in jobs)
            {
                Console.WriteLine($"Job Id: {job.Key}, InvocationData: {job.Value.InvocationData}, Arguments: {job.Value}");
            }
            
            //SqlConnection _connection = new(_connectionString);
            //_connection.Open();
            //SqlCommand cmd = new(@"SELECT * FROM easyautodb.HangFire.Job", _connection);
            //using (SqlDataReader reader = cmd.ExecuteReader())
            //{
            //    if (reader.Read())
            //    {
            //        Console.WriteLine(string.Format("{0} | {1} | {2}", reader["id"], reader["InvocationData"], reader["Arguments"]));
            //    }
            //}
            //_connection.Close();
        }

    }
}
