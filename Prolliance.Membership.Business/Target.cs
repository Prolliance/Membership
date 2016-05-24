using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 权限对象
    /// </summary>
    public class Target : ModelBase
    {
        internal static DataRepo<TargetInfo> TargetInfoRepo = new DataRepo<TargetInfo>();
        internal static DataRepo<OperationInfo> OperationInfoRepo = new DataRepo<OperationInfo>();

        #region 属性

        /// <summary>
        /// Key
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 作用域动态表达式
        /// </summary>
        public string Scope { get; set; }
        public string AppKey
        {
            get
            {
                return this.GetApp().Key;
            }
        }

        #endregion

        #region 静态方法
        /// <summary>
        /// 创建一个新的 Target
        /// </summary>
        public static Target Create()
        {
            Target target = new Target();
            return target;
        }
        public static Target GetTargetById(string targetId)
        {
            return TargetInfoRepo
                .Read()
                .FirstOrDefault(tag => tag.Id == targetId)
                .MappingTo<Target>();
        }
        public static Target GetTarget(string appKey, string targetCode)
        {
            App app = App.GetApp(appKey);
            if (app == null) return null;
            return TargetInfoRepo
                .Read()
                .FirstOrDefault(tag => tag.AppId == app.Id
                    && tag.Code == targetCode)
                .MappingTo<Target>();
        }
        public static List<Target> GetTargetList()
        {
            return TargetInfoRepo
                .Read().MappingToList<Target>();
        }
        #endregion

        #region 普通实例方法
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            List<Operation> operationList = this.OperationList;
            foreach (Operation operation in operationList)
            {
                operation.Delete();
            }
            TargetInfoRepo.Delete(this.MappingTo<TargetInfo>());
        }
        public void Save()
        {
            TargetInfo targetInfo = TargetInfoRepo.Read().FirstOrDefault(tag => tag.AppId == this.AppId
                && tag.Code == this.Code);
            if (targetInfo != null && targetInfo.Id != this.Id)
            {
                throw new Exception(string.Format("保存‘{0}’对象时，发现唯一键冲突", this.GetType().Name));
            }
            TargetInfoRepo.Save(this.MappingTo<TargetInfo>());
        }
        #endregion

        #region 有关App
        App _App;
        public App GetApp()
        {
            if (_App == null)
            {
                _App = App.GetAppById(this.AppId);
            }
            return _App;
        }
        #endregion

        #region 有关操作
        List<Operation> _OperationList;
        /// <summary>
        /// 操作列表
        /// </summary>
        public List<Operation> OperationList
        {
            get
            {
                if (_OperationList == null)
                {
                    _OperationList = OperationInfoRepo.Read()
                        .Where(operation => operation.TargetId == this.Id
                        && operation.AppId == this.AppId)
                        .ToList()
                        .MappingToList<Operation>();
                }
                return _OperationList;
            }
        }
        public Operation AddOperation(Operation operation)
        {
            operation.AppId = this.AppId;
            operation.TargetId = this.Id;
            operation.Save();
            return operation;
        }
        public Operation RemoveOperation(Operation operation)
        {
            operation.AppId = this.AppId;
            operation.TargetId = this.Id;
            operation.Delete();
            return operation;
        }
        #endregion
    }
}

