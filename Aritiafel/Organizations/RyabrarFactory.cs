using Aritiafel.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Aritiafel.Organizations
{
    //創建只需要一個物件的工廠，有需要直接獲取
    public static class RyabrarFactory
    {
        static ConcurrentDictionary<ArProductInfo, object> _Products;
        public static T CreateOrGet<T>(params object[] Args)
        {
            ArProductInfo info = new ArProductInfo(typeof(T), Args);
            return (T)CreateOrGetObject(info);
        }
        public static object CreateOrGet(ArProductInfo info)
            => CreateOrGetObject(info);
        public static T CreateOrGet<T>(ArProductInfo info)
            => (T)CreateOrGetObject(info);
        internal static object CreateOrGetObject(ArProductInfo info)
        {
            object result;
            if(_Products.TryGetValue(info, out result))
            {
                if (_Products[info] != null)
                    return result;
                else
                    _Products.TryRemove(info, out _);
            }
                
            if (info.Args.Count == 0)
                result = Activator.CreateInstance(info.Type);
            else
                result = Activator.CreateInstance(info.Type, info.Args.ToArray());

            if(result != null && info.Setting != null)
            {
                foreach(KeyValuePair<string, object> kvp in info.Setting)
                {
                    PropertyInfo pi = info.Type.GetProperty(kvp.Key);
                    if (pi != null) 
                        pi.SetValue(result, kvp.Value);
                }
            }

            if (result != null)
                _Products.TryAdd(info, result);

            return result;
        }
    }
}
