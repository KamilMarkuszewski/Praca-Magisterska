using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interfaces;
using UnityEngine;

namespace Core.Interfaces
{
    public class ServiceLocator
    {
        // map that contains pairs of interfaces and
        // references to concrete implementations
        private static IDictionary<object, object> services;
        private static bool started = false;

        internal static void Create()
        {
            services = new Dictionary<object, object>();

            // fill the map
            started = true;
        }

        public static T GetService<T>()
        {
            try
            {
                if (started == false)
                {
                    Create();
                    Debug.Log("InitServices");
                }
                return (T)services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("The requested service is not registered");
            }
        }

        public static IDictionary<object, object> GetDictionary()
        {
            try
            {
                if (started == false)
                {
                    Create();
                }
                return services;
            }
            catch (KeyNotFoundException)
            {
                throw new NotImplementedException();
            }
        }
    }
}
