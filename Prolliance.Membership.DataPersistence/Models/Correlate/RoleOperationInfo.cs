using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 角色权限关系表模型（唯一条件：RoleCode + AppKey + TargetCode + OperationCode）
    /// </summary>
    public class RoleOperationInfo : IModel
    {
        public string Id { get; set; }
        [ModelIdKey]
        public string AppId { get; set; }
        [ModelIdKey]
        public string RoleId { get; set; }
        [ModelIdKey]
        public string TargetId { get; set; }
        [ModelIdKey]
        public string OperationId { get; set; }
    }
}
