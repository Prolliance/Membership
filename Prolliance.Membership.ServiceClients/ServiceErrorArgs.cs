using System;

namespace Prolliance.Membership.ServiceClients
{
    public class ServiceErrorException : Exception
    {
        /// <summary>
        /// 服务返回的状态
        /// </summary>
        public ServiceState State { get; set; }
        public ServiceErrorException()
        { }

        public ServiceErrorException(string message)
            : base(message) { }

        public ServiceErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
