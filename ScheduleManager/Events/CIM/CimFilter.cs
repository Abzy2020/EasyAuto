using Microsoft.Management.Infrastructure;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EasyAuto.Events.CIM
{
    [SupportedOSPlatform("windows")]
    internal class CimFilter : CimInstance
    {
        public string FilterName;                               // name of filter
        private string QueryLanguage = "WQL";                   // query language option
        protected string Query;                                 // wql query
        protected string FilterDescription;
        readonly ManagementClass __EventFilter;                 // __EventFilter class
        private ManagementObject __EventFilterInstance;         // __EventFilter class instance



        public CimFilter(string FilterName, string FilterDescription) : base(@"root\subscription:__EventFilter")
        {
            // the name of the filter
            this.FilterName = FilterName;
            this.FilterDescription = FilterDescription;
            // filter class to narrow events
            //__EventFilter = new ManagementClass(@"root\subscription:__EventFilter");
            //__EventFilterInstance = __EventFilter.CreateInstance();
        }


        // lets user build wql query for registration of instance creation events to cim classes
        public void BuildQuery(string pollingInterval, string targetClass, string property, string conditionValue)
        {
            Query = $"SELECT * FROM __InstanceCreationEvent WITHIN {pollingInterval} WHERE TargetInstance ISA '{targetClass}' AND TargetInstance.{property} = '{conditionValue}'";
            Console.WriteLine(Query);
        }



        // lets user build wql query for registration of instance creation events to cim classes
        public void BuildQuery(string pollingInterval, string targetClass)
        {
            Query = $"SELECT * FROM __InstanceCreationEvent WITHIN {pollingInterval} WHERE TargetInstance ISA '{targetClass}'";
            Console.WriteLine(Query);
        }



        // creates the filter that will be binded to the consumer
        public void RegisterFilter()
        {
            // configure desired properties
            //__EventFilterInstance["Name"] = FilterName;
            //__EventFilterInstance["QueryLanguage"] = QueryLanguage;
            //__EventFilterInstance["Query"] = Query;
            //__EventFilterInstance.Put();

            cimInstance["Name"] = FilterName;
            cimInstance["QueryLanguage"] = QueryLanguage;
            cimInstance["Query"] = Query;
            cimInstance.Put();
        }


    }
}
