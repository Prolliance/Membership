using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prolliance.Membership.Business.Utils;
using Prolliance.Membership.DataTransfer;

namespace Prolliance.Membership.ServicePoint.service
{
    /// <summary>
    /// cache 的摘要说明
    /// </summary>
    public class cache : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var cachekey = context.Request["cachekey"];
            if (string.IsNullOrEmpty(cachekey))
            {
                AppAdapter.RemoveAllCache();
            }
            else
            {
                CacheSync.Rebuild(cachekey);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}