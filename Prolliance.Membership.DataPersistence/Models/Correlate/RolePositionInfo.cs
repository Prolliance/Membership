using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 角色岗位系表模型（唯一条件：RoleCode + OrganizationCode + PositionCode）
    /// </summary>
    public class RolePositionInfo : IModel
    {
        public string Id { get; set; }
        [ModelIdKey]
        public string RoleId { get; set; }
        [ModelIdKey]
        public string OrganizationId { get; set; }
        [ModelIdKey]
        public string PositionId { get; set; }
    }
}
