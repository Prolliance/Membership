using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Prolliance.EasyCache;
using Prolliance.Membership.DataPersistence;
using TpNet.Extends;

namespace Prolliance.Membership.Business.Utils
{
    public static class CacheSync
    {
        private static ICache Cache = CacheManager.Create();

        public static void Rebuild(string key)
        {
            Assembly ass = Assembly.Load("Prolliance.Membership.DataPersistence");
            Type type = ass.GetType(key);
            //type.MakeGenericType()
            //object o = Activator.CreateInstance(type);
            Type typeDao = typeof (DataRepo<>);
            typeDao= typeDao.MakeGenericType(type);
            object o = Activator.CreateInstance(typeDao);
            o.InvokeMethod("Build", null);
        }
    }
}
