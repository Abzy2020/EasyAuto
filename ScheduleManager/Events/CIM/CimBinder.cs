using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuto.Events.CIM
{
    [SupportedOSPlatform("windows")]
    internal class CimBinder : CimInstance
    {
        private ManagementClass bindingClass;           // class
        private ManagementObject bindingInstance;       // instance of the class


        public CimBinder() : base(@"root\subscription:__FilterToConsumerBinding") 
        {
            bindingClass = new ManagementClass(@"root\subscription:__FilterToConsumerBinding");
            bindingInstance = bindingClass.CreateInstance();
        }


        // establishes permanent watcher of events qualified to pass through the filter and the consumer 
        public void CreateInstanceBinder(CimFilter filter, CimConsumer consumer)
        {         
            bindingInstance["Filter"] = filter.GetClassPath();
            bindingInstance["Consumer"] = consumer.GetClassPath();
            bindingInstance.Put();
        }
    }
}
