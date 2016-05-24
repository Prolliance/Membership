using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataTransfer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.DataTransfer
{
    public static class AppAdapter
    {
        public static bool CheckApp(string appKey, string secret)
        {
            if (appKey == AppSettings.Name)
            {
                return false;
            }
            App app = App.GetApp(appKey);
            return app != null && app.Secret == secret && app.IsActive;
        }

        public static void RemoveAllCache()
        {
            App.RemoveAllCache();
        }

        public static void ExecuteDynamicSql(string dynamicSql)
        {
            App.ExecuteDynamicSql(dynamicSql);
        }

        public static void ImportManifestText(string appKey, string manifest)
        {
            if (string.IsNullOrWhiteSpace(appKey)) return;
            App app = App.GetApp(appKey);
            if (app != null)
            {
                app.ImportManifestText(manifest);
            }
        }

        public static void ImportManifest(string appKey, List<TargetInfo> targetList)
        {
            if (string.IsNullOrWhiteSpace(appKey)) return;
            App app = App.GetApp(appKey);
            if (app != null)
            {
                app.ImportManifest(targetList.MappingToList<App.TargetPetty>());
            }
        }

        public static List<AppInfo> GetAppList()
        {
            var appLst = App.GetAppList();
            if(appLst == null)
            {
                return new List<AppInfo>();
            }
            return appLst.MappingToList<AppInfo>();
        }

        public static List<TargetInfo> GetTargetList(string appKey)
        {
            if (string.IsNullOrWhiteSpace(appKey)) return null;
            App app = App.GetApp(appKey);
            if (app == null) return null;
            return app.TargetList.MappingToList<TargetInfo>();
        }

        public static List<OperationInfo> GetOperationList()
        {
            var operationList = Operation.GetOperationList();
            if (operationList == null) 
                return new List<OperationInfo>();
            return operationList.MappingToList<OperationInfo>();
        }

        public static void UpdateTargetList(string appKey, List<TargetInfo> targetList)
        {
            if (string.IsNullOrWhiteSpace(appKey)) return;
            App app = App.GetApp(appKey);
            if (app == null) return;
            var oriList = app.TargetList.MappingToList<App.TargetPetty>();
            oriList.RemoveAll(t => targetList.Select(nt => nt.Code).ToList().Contains(t.Code));
            oriList.AddRange(targetList.MappingToList<App.TargetPetty>());
            app.ImportManifest(oriList);
        }

        public static void AddTargetList(string appKey, List<TargetInfo> targetList)
        {
            if (string.IsNullOrWhiteSpace(appKey) || targetList == null) return;
            App app = App.GetApp(appKey);
            if (app == null) return;
            foreach (var targetInfo in targetList)
            {
                if (targetInfo == null) continue;
                var target = app.TargetList.FirstOrDefault(t => t.Id == targetInfo.Id
                    || t.Code == targetInfo.Code);
                //如果不存在则添加
                if (target == null)
                {
                    target = targetInfo.MappingTo<Target>(new List<string>() { "AppKey", "OperationList" });
                    target = app.AddTarget(target);
                }
                //处理opeation
                if (targetInfo.OperationList == null) continue;
                foreach (OperationInfo operationInfo in targetInfo.OperationList)
                {
                    var operation = target.OperationList
                        .FirstOrDefault(o => o.Id == operationInfo.Id
                        || (o.Code == operationInfo.Code));
                    //如果不存在则添加
                    if (operation == null)
                    {
                        target.AddOperation(operationInfo.MappingTo<Operation>());
                    }
                }
            }
        }

        public static void DeleteTargetList(string appKey, List<TargetInfo> targetList)
        {
            if (string.IsNullOrWhiteSpace(appKey) || targetList == null) return;
            App app = App.GetApp(appKey);
            if (app == null || app.TargetList == null) return;
            foreach (var targetInfo in targetList)
            {
                if (targetInfo == null) continue;
                var target = app.TargetList.FirstOrDefault(t => t.Id == targetInfo.Id
                    || (t.AppKey == targetInfo.AppKey && t.Code == targetInfo.Code)
                    || (t.AppId == targetInfo.AppId && t.Code == targetInfo.Code));
                if (target != null)
                {
                    app.RemoveTarget(target);
                }
            }
        }

        public static void AddOperationList(string appKey, List<OperationInfo> operationList)
        {
            if (string.IsNullOrWhiteSpace(appKey) || operationList == null) return;
            App app = App.GetApp(appKey);
            if (app == null || app.TargetList == null) return;
            foreach (var operationInfo in operationList)
            {
                var target = app.TargetList
                    .FirstOrDefault(t => t.Id == operationInfo.TargetId
                    || t.Code == operationInfo.TargetCode);
                if (target != null)
                {
                    target.AddOperation(operationInfo.MappingTo<Operation>(new List<string>() { "AppKey", "TargetCode" }));
                }
            }
        }

        public static void DeleteOperationList(string appKey, List<OperationInfo> operationList)
        {
            if (string.IsNullOrWhiteSpace(appKey) || operationList == null) return;
            App app = App.GetApp(appKey);
            if (app == null || app.TargetList == null) return;
            foreach (var operationInfo in operationList)
            {
                var target = app.TargetList
                    .FirstOrDefault(t => t.Id == operationInfo.TargetId
                    || t.Code == operationInfo.TargetCode);
                if (target != null)
                {
                    Operation operation = target.OperationList.FirstOrDefault(o => o.Code == operationInfo.Code);
                    if (operation != null)
                    {
                        target.RemoveOperation(operation);
                    }
                }
            }
        }
    }
}
