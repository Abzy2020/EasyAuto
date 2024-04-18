using Microsoft.Management.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuto.Events.CIM
{
    [SupportedOSPlatform("windows")]
    internal abstract class CimInstance
    {
        public ManagementClass cimClass;
        public ManagementObject cimInstance;
        public ManagementObjectCollection cimInstances;
        public IList<CimProperty> cimProperties = new List<CimProperty>();
        public string Name;

        public CimInstance(string ClassName)
        {
            Name = ClassName;
            cimClass = new ManagementClass(ClassName);
            cimInstance = cimClass.CreateInstance();
            //cimInstances = cimClass.GetInstances();
        }


        // returns the location of the instantiated path for binding
        public string GetClassPath()
        {
            return cimInstance.ClassPath.Path;
        }


        // gets instance of class
        public void GetInstances()
        {
            cimInstances = cimClass.GetInstances();
        }


        // adds CIM class properties to list of properties for ease of use
        public void GetProperties()
        {
            if (cimInstances.Count == 0)
            {
                Console.WriteLine("No instances to fetch properties from.");
                return;
            }
            if (cimProperties.Count == 0)
            {
                Console.WriteLine("Could not fetch properties.");
                return;
            }
            GetInstances();
            foreach (var instance in cimInstances)
            {
                PropertyDataCollection classProperties = instance.Properties;

                foreach (var property in classProperties)
                {
                    cimProperties.Add(
                        new CimProperty
                        {
                            Name = property.Name,
                            Value = property.Value
                        }
                    );
                }
            }
        }


        // gets a single property value
        public object GetPropertyValue(string propertyName)
        {
            object propertyValue = "";
            foreach (var property in cimProperties)
            {
                if (property.Name == propertyName)
                {
                    propertyValue = property.Value;
                }
            }
            return propertyValue;
        }


        // prints property keys and values related to CIM class
        public void PrintInfo()
        {
            if (cimInstances.Count == 0)
            {
                GetInstances();
            }
            if (cimProperties.Count == 0)
            {
                GetProperties();
            }
            foreach (var instance in cimInstances)
            {
                foreach (var property in cimProperties)
                {
                    Console.WriteLine($"{property.Name}: {property.Value}");
                }
            }
        }
    }
}
