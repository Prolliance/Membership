using AjaxEngine.Extends;
using AjaxEngine.Serializes;
using Prolliance.Membership.Common;
using Prolliance.Membership.ServiceClients.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Prolliance.Membership.ServiceClients.Manifests
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public abstract class AppManifestBase : Attribute
    {
        #region 内部类型
        /// <summary>
        /// 用于批量导入轻量级 Target 序列化清单类型
        /// </summary>
        public class TargetLite
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Scope { get; set; }
            public string Group { get; set; }
            public string Summary { get; set; }
            public List<OperationLite> OperationList { get; set; }
            public TargetLite()
            {
                this.OperationList = new List<OperationLite>();
            }
        }

        /// <summary>
        /// 用于批量导入轻量级 Operation 序列化清单类型
        /// </summary>
        public class OperationLite
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
        }
        #endregion

        internal App _App { get; set; }

        internal static JsonSerializer _Serializer = new JsonSerializer();

        internal List<TargetManifestBase> _TargetManifestList { get; set; }

        internal List<Operation> _OperationList
        {
            get
            {
                List<Operation> operationList = new List<Operation>();
                if (_TargetManifestList == null) return operationList;
                foreach (var targetManifest in _TargetManifestList)
                {
                    operationList.AddRange(targetManifest._OperationList);
                }
                return operationList;
            }
        }

        public App GetApp() { return _App; }
        public List<TargetManifestBase> GetTargetManifestList() { return _TargetManifestList; }
        public List<Operation> GetOperationList() { return _OperationList; }

        public AppManifestBase()
        {
            Type type = this.GetType();
            this._TargetManifestList = new List<TargetManifestBase>();
            this._App = type.GetAttribute<App>();
            if (this._App == null)
            {
                throw new Exception(string.Format("没有找到‘{0}’的清单特性", type.FullName));
            }
            if (ServiceClient.Options != null
                && !string.IsNullOrWhiteSpace(ServiceClient.Options.AppKey))
            {
                this._App.Key = ServiceClient.Options.AppKey;
            }
            PropertyInfo[] propertyList = this.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                TargetManifestBase targetManifest = property.GetAttribute<TargetManifestBase>();
                if (targetManifest != null)
                {
                    targetManifest._Target.AppKey = this._App.Key;
                    List<Operation> operationList = targetManifest._OperationList;
                    foreach (Operation operation in operationList)
                    {
                        operation.AppKey = this._App.Key;
                    }
                    this.SetPropertyValue(property.Name, targetManifest);
                    this._TargetManifestList.Add(targetManifest);
                }
            }
        }

        /// <summary>
        /// 导出权限清单
        /// </summary>
        /// <returns></returns>
        private List<TargetLite> ExportManifest()
        {
            List<TargetLite> targetLiteList = new List<TargetLite>();
            List<TargetManifestBase> targetManifestList = this._TargetManifestList;
            foreach (TargetManifestBase targetManifest in targetManifestList)
            {
                if (targetManifest != null)
                {
                    var targetLite = targetManifest._Target.MappingTo<TargetLite>(new List<string> { "OperationList" });
                    var operationList = targetManifest._OperationList;
                    foreach (Operation operation in operationList)
                    {
                        targetLite.OperationList.Add(operation.MappingTo<OperationLite>());
                    }
                    targetLiteList.Add(targetLite);
                }
            }
            return targetLiteList;
        }

        /// <summary>
        /// 动态权限清单
        /// </summary>
        /// <returns></returns>
        public virtual List<TargetLite> DynamicTargetList
        {
            get
            {
                return new List<TargetLite>();
            }
        }

        /// <summary>
        /// 导出权限清单 JSON
        /// </summary>
        /// <returns></returns>
        public string ExportManifestText()
        {
            var targetLiteList = new List<TargetLite>();
            //static
            var staticTargetLiteList = this.ExportManifest();
            if (staticTargetLiteList != null)
            {
                targetLiteList.AddRange(staticTargetLiteList);
            }
            //dynamic
            var dynamicTargetLiteList = this.DynamicTargetList;
            if (dynamicTargetLiteList != null)
            {
                targetLiteList.AddRange(dynamicTargetLiteList);
            }
            return _Serializer.Serialize(targetLiteList);
        }
    }
}
