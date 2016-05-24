using AjaxEngine.Extends;
using Prolliance.Membership.ServiceClients.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Prolliance.Membership.ServiceClients.Manifests
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public abstract class TargetManifestBase : Attribute
    {
        internal Target _Target { get; set; }

        internal List<Operation> _OperationList { get; set; }

        public Target GetApp() { return _Target; }
        public List<Operation> GetOperationList() { return _OperationList; }

        /// <summary>
        /// 默认构造
        /// </summary>
        protected TargetManifestBase()
        {
            Type type = this.GetType();
            this._OperationList = new List<Operation>();
            this._Target = type.GetAttribute<Target>();
            if (this._Target == null)
            {
                throw new Exception(string.Format("没有找到‘{0}’的清单特性", type.FullName));
            }
            if (ServiceClient.Options != null
                && !string.IsNullOrWhiteSpace(ServiceClient.Options.AppKey))
            {
                this._Target.AppKey = ServiceClient.Options.AppKey;
            }
            PropertyInfo[] propertyList = this.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                Operation operation = property.GetAttribute<Operation>();
                if (operation != null)
                {
                    operation.AppKey = this._Target.AppKey;
                    operation.TargetCode = this._Target.Code;
                    this.SetPropertyValue(property.Name, operation);
                    this._OperationList.Add(operation);
                }
            }
        }
    }
}
