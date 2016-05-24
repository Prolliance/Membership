using Amuse.Serializes;
using Prolliance.EasyCache;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 应用
    /// </summary>
    /// <code></code>
    public class App : ModelBase
    {
        private static ICache Cache = CacheManager.Create();
        internal static DataRepo<AppInfo> AppInfoRepo = new DataRepo<AppInfo>();
        internal static DataRepo<TargetInfo> TargetInfoRepo = new DataRepo<TargetInfo>();
        internal static DataRepo<OperationInfo> OperationInfoRepo = new DataRepo<OperationInfo>();
        internal static DataRepo<RoleInfo> RoleInfoRepo = new DataRepo<RoleInfo>();
        internal static JsonSerializer Serializer = new JsonSerializer();

        #region 内部类型
        /// <summary>
        /// 用于批量导入轻量级 Target 序列化清单类型
        /// </summary>
        public class TargetPetty
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Scope { get; set; }
            public string Group { get; set; }
            public string Summary { get; set; }
            public List<OperationPetty> OperationList { get; set; }
            public TargetPetty()
            {
                this.OperationList = new List<OperationPetty>();
            }
        }

        /// <summary>
        /// 用于批量导入轻量级 Operation 序列化清单类型
        /// </summary>
        public class OperationPetty
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 代码
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 白名单列表
        /// </summary>
        public string WhiteList { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }
        public int Sort { get; set; }
        #endregion

        #region 静态方法
        /// <summary>
        /// 获得所有应用
        /// </summary>
        /// <returns></returns>
        public static List<App> GetAppList()
        {
            return AppInfoRepo.Read()
                .OrderByDescending(app => app.Sort)
                .MappingToList<App>();
        }

        /// <summary>
        /// 获取App
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static App GetApp(string key)
        {
            return GetAppList().FirstOrDefault(a => a.Key == key);
        }
        public static App GetAppById(string id)
        {
            return GetAppList().FirstOrDefault(a => a.Id == id);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public static App Create()
        {
            App app = new App();
            app.Key = StringFactory.NewGuid();
            app.Secret = StringFactory.NewGuid();
            return app;
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            Cache.RemoveAll();
        }

        /// <summary>
        /// 动态执行SQL
        /// </summary>
        /// <param name="dynamicSql"></param>
        public static void ExecuteDynamicSql(string dynamicSql)
        {
            AppInfoRepo.ExecuteDynamicSql(dynamicSql);
        }
        #endregion

        #region 实列方法
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            AppInfoRepo.Delete(this.MappingTo<AppInfo>());
            //移除应用权限对象
            TargetInfoRepo.Read()
                .Where(tag => tag.AppId == this.Id)
                .ToList()
                .ForEach(tag => TargetInfoRepo.Delete(tag));
            //移除应用权限操作
            OperationInfoRepo.Read()
                .Where(op => op.AppId == this.Id)
                .ToList()
                .ForEach(op => OperationInfoRepo.Delete(op));
        }

        public void Save()
        {
            App app = GetApp(this.Key);
            if (app != null && app.Id != this.Id)
            {
                throw new Exception(string.Format("保存‘{0}’对象时，发现唯一键冲突", this.GetType().Name));
            }
            AppInfoRepo.Save(this.MappingTo<AppInfo>());
        }

        private static object ImportLocker = new object();

        /// <summary>
        /// 导入权限清单
        /// </summary>
        /// <param name="targetPettyList">清单对象列表</param>
        public void ImportManifest(List<TargetPetty> targetPettyList)
        {
            lock (ImportLocker)
            {
                //清除数据库中有且清单中没有的 Target
                List<Target> targetList = this.TargetList;
                if (targetList != null)
                {
                    foreach (Target target in targetList)
                    {
                        if (!targetPettyList.Exists(tag => tag.Code == target.Code))
                        {
                            target.Delete();//会级联清除 Operation
                        }
                    }
                }
                //更新或添加清单中的 Target
                foreach (TargetPetty targetPetty in targetPettyList)
                {
                    //保存或更新 Target
                    Target target = Target.GetTarget(this.Key, targetPetty.Code) ?? new Target();
                    Mapper.Clone<TargetPetty, Target>(targetPetty, ref target, new List<string> { "OperationList", "Id", "AppKey" });
                    target.AppId = this.Id;
                    target.Sort = targetPettyList.Count() - 1 - targetPettyList.IndexOf(targetPetty);
                    target.Save();

                    #region 处理单个 Target 下的 Operation
                    //清除数据库中有且清单中没有的 Operation
                    List<Operation> operationList = target.OperationList;
                    if (operationList != null)
                    {
                        foreach (Operation operation in operationList)
                        {
                            if (!targetPetty.OperationList.Exists(op => op.Code == operation.Code))
                            {
                                operation.AppId = this.Id;
                                operation.TargetId = target.Id;
                                operation.Delete();
                            }
                        }
                    }
                    //更新或添加清单中的 Operation
                    if (targetPetty.OperationList != null)
                    {
                        foreach (OperationPetty operationPetty in targetPetty.OperationList)
                        {
                            Operation operation = Operation.GetOperation(this.Key,
                                targetPetty.Code,
                                operationPetty.Code)
                                ?? new Operation();
                            Mapper.Clone<OperationPetty, Operation>(operationPetty, ref operation, new List<string> { "Id", "AppKey", "TargetCode" });
                            operation.AppId = this.Id;
                            operation.TargetId = target.Id;
                            operation.Save();
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 导出权限清单
        /// </summary>
        public List<TargetPetty> ExportManifest()
        {
            var targetList = this.TargetList;
            List<App.TargetPetty> targetManifestList = new List<App.TargetPetty>();
            foreach (var target in targetList)
            {
                App.TargetPetty targetManifest = target.MappingTo<App.TargetPetty>(new List<string> { "OperationList" });
                targetManifest.OperationList = target.OperationList.MappingToList<App.OperationPetty>();
                targetManifestList.Add(targetManifest);
            }
            return targetManifestList;
        }

        /// <summary>
        /// 导入权限清单
        /// </summary>
        /// <param name="manifrst">清单JSON文本</param>
        public void ImportManifestText(string manifrst)
        {
            var targetManifestList = Serializer.Deserialize<List<TargetPetty>>(manifrst);
            ImportManifest(targetManifestList);
        }

        /// <summary>
        /// 导出权限清单
        /// </summary>
        public string ExportManifestText()
        {
            var targetManifestList = this.ExportManifest();
            return Serializer.Serialize(targetManifestList);
        }
        #endregion

        #region 权限对象

        List<Target> _TargetList;
        /// <summary>
        /// 权限对象集合
        /// </summary>
        public List<Target> TargetList
        {
            get
            {
                if (_TargetList == null)
                {
                    _TargetList = TargetInfoRepo.Read().Where(target => target.AppId == this.Id)
                        .OrderByDescending(target => target.Sort)
                        .ToList().MappingToList<Target>();
                }
                return _TargetList;
            }
        }
        public Target AddTarget(Target target)
        {
            target.AppId = this.Id;
            target.Save();
            return target;
        }
        public Target RemoveTarget(Target target)
        {
            target.AppId = this.Id;
            target.Delete();
            return target;
        }
        #endregion
    }
}

