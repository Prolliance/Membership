using AjaxEngine.Utils;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataTransfer;
using Prolliance.Membership.DataTransfer.Models;
using Prolliance.Membership.DataTransfer.Utils;
using Prolliance.Membership.ServicePoint.Lib.Services;
using System.Collections.Generic;

namespace Prolliance.Membership.ServicePoint.Service
{
    /// <summary>
    /// App 的摘要说明
    /// </summary>
    [Summary(Name = "应用服务", Description = "应用相关 API，必须用接入应用的身份访问。")]
    [ServiceAuth(Type = ServiceAuthType.App, IgnoreMethods = new string[] { "RemoveAllCache", "ExecuteDynamicSql" })]
    public class AppService : ServiceBase
    {
        [Summary(Description = "清除所有缓存")]
        [AjaxMethod]
        public ServiceResult<object> RemoveAllCache()
        {
            AppAdapter.RemoveAllCache();
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "动态执行SQL")]
        [AjaxMethod]
        public ServiceResult<object> ExecuteDynamicSql(string dynamicSql)
        {
            AppAdapter.ExecuteDynamicSql(dynamicSql);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "导入应用权限清单文本（会影响参数中不包括的 Target 对象，但不会影响保留的 Target 对象对应已有的权限配置）", Parameters = "manifest:清单JSON文本")]
        [AjaxMethod]
        public ServiceResult<object> ImportManifestText(string manifest)
        {
            //系统本身不允许通过 Service 导入权限清单配置
            if (this.AppKey == AppSettings.Name)
            {
                return new ServiceResult<object>(ServiceState.InvalidAuth);
            }
            //
            AppAdapter.ImportManifestText(this.AppKey, manifest);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "导入应用权限清单对象（会影响参数中不包括的 Target 对象，但不会影响保留的 Target 对象对应已有的权限配置）", Parameters = "targetList:清单对象")]
        [AjaxMethod]
        public ServiceResult<object> ImportManifest(List<TargetInfo> targetList)
        {
            //系统本身不允许通过 Service 导入权限清单配置
            if (this.AppKey == AppSettings.Name)
            {
                return new ServiceResult<object>(ServiceState.InvalidAuth);
            }
            //
            AppAdapter.ImportManifest(this.AppKey, targetList);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "获取所有应用")]
        [AjaxMethod]
        public ServiceResult<List<AppInfo>> GetAppList()
        {
            return new ServiceResult<List<AppInfo>>(AppAdapter.GetAppList());
        }

        [Summary(Description = "获取当前应用的所有权限对象")]
        [AjaxMethod]
        public ServiceResult<List<TargetInfo>> GetTargetList(string otherAppKey)
        {
            otherAppKey = string.IsNullOrEmpty(otherAppKey) ? this.AppKey : otherAppKey;

            return new ServiceResult<List<TargetInfo>>(AppAdapter.GetTargetList(otherAppKey));
        }

        [Summary(Description = "获取所有操作权限")]
        [AjaxMethod]
        public ServiceResult<List<OperationInfo>> GetOperationList()
        {
            return new ServiceResult<List<OperationInfo>>(AppAdapter.GetOperationList());
        }

        [Summary(Description = "更新一组权限对象（不会影响参数中不包括的 Target 对象，也不会影响已有的权限配置）")]
        [AjaxMethod]
        public ServiceResult<object> UpdateTargetList(List<TargetInfo> targetList)
        {
            if (this.AppKey == AppSettings.Name)
            {
                return new ServiceResult<object>(ServiceState.InvalidAuth);
            }
            AppAdapter.UpdateTargetList(this.AppKey, targetList);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "添加一组权限对象")]
        [AjaxMethod]
        public ServiceResult<object> AddTargetList(List<TargetInfo> targetList)
        {
            if (this.AppKey == AppSettings.Name)
            {
                return new ServiceResult<object>(ServiceState.InvalidAuth);
            }
            AppAdapter.AddTargetList(this.AppKey, targetList);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "删除一组权限对象")]
        [AjaxMethod]
        public ServiceResult<object> DeleteTargetList(List<TargetInfo> targetList)
        {
            if (this.AppKey == AppSettings.Name)
            {
                return new ServiceResult<object>(ServiceState.InvalidAuth);
            }
            AppAdapter.DeleteTargetList(this.AppKey, targetList);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "添加一组权限操作")]
        [AjaxMethod]
        public ServiceResult<object> AddOperationList(List<OperationInfo> operationList)
        {
            if (this.AppKey == AppSettings.Name)
            {
                return new ServiceResult<object>(ServiceState.InvalidAuth);
            }
            AppAdapter.AddOperationList(this.AppKey, operationList);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "删除一组权限操作")]
        [AjaxMethod]
        public ServiceResult<object> DeleteOperationList(List<OperationInfo> operationList)
        {
            if (this.AppKey == AppSettings.Name)
            {
                return new ServiceResult<object>(ServiceState.InvalidAuth);
            }
            AppAdapter.DeleteOperationList(this.AppKey, operationList);
            return new ServiceResult<object>(null);
        }


    }
}