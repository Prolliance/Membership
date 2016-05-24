using System;

namespace Prolliance.Membership.ServicePoint.Lib.Services
{
    public class ServiceAuth : Attribute
    {
        public ServiceAuthType Type { get; set; }
        public string[] IgnoreMethods { get; set; }
        public ServiceAuth()
        {
        }
    }
}
