using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 针对权限对象的操作
    /// </summary>
    public class Operation : ModelBase
    {
        internal static DataRepo<OperationInfo> OperationInfoRepo = new DataRepo<OperationInfo>();
        internal static DataRepo<RoleOperationInfo> RoleOperationInfoRepo = new DataRepo<RoleOperationInfo>();

        #region 属性
        public string AppId { get; set; }
        /// <summary>
        /// TargetCode 用于指定操作属于那个权限对象，必须指定
        /// </summary>
        public string TargetId { get; set; }
        /// <summary>
        /// Code 是指操作的编码，在同一个 TargetCode 下必须唯一，必须指定
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Name 为操作的名称，可以重复，必须指定
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 操作的说明信息，通常可以省略
        /// </summary>
        public string Summary { get; set; }
        public string AppKey
        {
            get
            {
                return this.GetApp().Key;
            }
        }
        public string TargetCode
        {
            get
            {
                return this.GetTarget().Code;
            }
        }
        #endregion

        #region 普通实例方法
        public Target GetTarget()
        {
            return Target.GetTargetById(this.TargetId);
        }
        public App GetApp()
        {
            return App.GetAppById(this.AppId);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            //删除和角色关系
            List<RoleOperationInfo> roList = RoleOperationInfoRepo
                .Read()
                .Where(ro => ro.OperationId == this.Id)
                .ToList();
            foreach (var ro in roList)
            {
                RoleOperationInfoRepo.Delete(ro);
            }
            OperationInfoRepo.Delete(this.MappingTo<OperationInfo>());
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            var operationInfo = OperationInfoRepo.Read()
                .FirstOrDefault(op => op.TargetId == this.TargetId
                && op.Code == this.Code);
            if (operationInfo != null && operationInfo.Id != this.Id)
            {
                throw new Exception(string.Format("保存‘{0}’对象时，发现唯一键冲突", this.GetType().Name));
            }
            OperationInfoRepo.Save(this.MappingTo<OperationInfo>());
        }
        #endregion

        #region 静态方法
        public static Operation GetOperation(string appKey, string targetCode, string operationCode)
        {
            App app = App.GetApp(appKey);
            Target target = Target.GetTarget(appKey, targetCode);
            if (app == null || target == null)
            {
                return null;
            }
            return OperationInfoRepo.Read()
                .FirstOrDefault(op => op.AppId == app.Id
                && op.TargetId == target.Id
                && op.Code == operationCode)
                .MappingTo<Operation>();
        }
        public static Operation GetOperationById(string id)
        {
            return OperationInfoRepo.Read()
                .FirstOrDefault(op => op.Id == id)
                .MappingTo<Operation>();
        }
        public static List<Operation> GetOperationList()
        {
            return OperationInfoRepo
                .Read().MappingToList<Operation>();
        }
        public static Operation Create()
        {
            Operation operation = new Operation();
            return operation;
        }
        #endregion
    }
}
